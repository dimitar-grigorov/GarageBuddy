namespace GarageBuddy.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Common.Constants;

    using GarageBuddy.Web.Infrastructure.Extensions;

    using Infrastructure.ViewRenderer;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Services.Data.Contracts;
    using Services.Messaging.Contracts;

    using ViewModels;
    using ViewModels.MailTemplates;
    using ViewModels.User;

    public class UserController : BaseController
    {
        private readonly IUserService userService;
        private readonly IEmailService emailService;
        private readonly IViewRenderer viewRenderer;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService userService,
            IEmailService emailService,
            IViewRenderer viewRenderer,
            ILogger<UserController> logger)
        {
            this.userService = userService;
            this.emailService = emailService;
            this.viewRenderer = viewRenderer;
            this.logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            var model = new RegisterFormModel();
            return this.View(model);
        }

        // [ValidateRecaptcha(Action = nameof(Register), ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                var result = await this.userService.RegisterWithEmailAsync(model.Email, model.Password);

                // TODO: Add confirmation email
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return this.View(model);
                }
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, Errors.SomethingWentWrong);
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
             var model = new LoginFormModel
            {
                ReturnUrl = returnUrl,
            };

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, Errors.InvalidUsernameOrPassword);
                return this.View(model);
            }

            try
            {
                var result = this.userService.LoginWithEmailAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (!result.Result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, Errors.InvalidUsernameOrPassword);

                    if (result.Result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, Errors.AccountLockedOut);
                    }

                    return this.View(model);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, Errors.InvalidUsernameOrPassword);
                return this.View(model);
            }

            if (model.ReturnUrl != null)
            {
                return Redirect(model.ReturnUrl);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                await this.userService.LogoutAsync();
                return this.RedirectToAction("Index", "Home");
            }

            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // [ValidateRecaptcha(Action = nameof(Register), ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return Json(new ModalFormResult(isValid: false, await this.RenderRazorViewToStringAsync("ForgotPassword", model)));
            }

            var result = await this.userService.GenerateEmailResetUriAsync(
                model.Email,
                GetOriginFromRequest(),
                Url.Action(nameof(ResetPassword), null, null)!,
                nameof(ResetPasswordFormModel.Token));

            if (!result.Succeeded)
            {
                foreach (var error in result.Messages)
                {
                    this.ModelState.AddModelError(string.Empty, error);
                }

                return Json(new ModalFormResult(isValid: false, await this.RenderRazorViewToStringAsync("ForgotPassword", model)));
            }

            var forgotPasswordViewModel = new ForgotPasswordMailViewModel
            {
                ResetPasswordUrl = result.Data,
                ApplicationName = GlobalConstants.SystemName,
            };

            var mailContent = await viewRenderer
                .RenderAsync(forgotPasswordViewModel, "ForgotPasswordEmailTemplate", GlobalConstants.MailTemplatePath);

            var mailResult = await emailService.SendResetPasswordEmail(model.Email, mailContent);

            if (!mailResult.Succeeded)
            {
                TempData[NotifyError] = Errors.SomethingWentWrong;
                return Json(new ModalFormResult(isValid: false, await this.RenderRazorViewToStringAsync("ForgotPassword", model)));
            }

            TempData[NotifySuccess] = Success.SendPasswordResetEmail;
            return Json(new ModalFormResult
            {
                IsValid = true,
                Html = await this.RenderRazorViewToStringAsync("ForgotPassword", model),
                RedirectUrl = Url.Action("Index", "Home")!,
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? token)
        {
            if (token == null)
            {
                return this.BadRequest();
            }

            var model = new ResetPasswordFormModel
            {
                Token = token,
            };

            return this.View(model);
        }

        // [ValidateRecaptcha(Action = nameof(Register), ValidationFailedAction = ValidationFailedAction.ContinueRequest)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.userService.ResetPasswordAsync(model.Email!, model.Password!, model.Token!);

            if (!result.Succeeded)
            {
                TempData[NotifyError] = result.Messages.FirstOrDefault();
                return this.View(model);
            }

            TempData[NotifySuccess] = result.Messages.FirstOrDefault();
            return this.RedirectToAction(nameof(Login));
        }

        private string GetOriginFromRequest()
        {
            return $"{Request.Scheme}://{Request.Host.Value}{Request.PathBase.Value}";
        }
    }
}
