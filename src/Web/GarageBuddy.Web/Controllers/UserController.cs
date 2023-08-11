namespace GarageBuddy.Web.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Common.Constants;

    using GarageBuddy.Web.Infrastructure.Extensions;

    using Infrastructure.ViewRenderer;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    using Services.Data.Contracts;
    using Services.Messaging.Contracts;

    using ViewModels.MailTemplates;
    using ViewModels.User;

    using static Common.Constants.ErrorMessageConstants;

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
                this.ModelState.AddModelError(string.Empty, ErrorSomethingWentWrong);
                return this.View(model);
            }

            return this.RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

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
                ModelState.AddModelError(string.Empty, ErrorInvalidUsernameOrPassword);
                return this.View(model);
            }

            try
            {
                var result = this.userService.LoginWithEmailAsync(
                    model.Email, model.Password, model.RememberMe, false);

                if (!result.Result.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, ErrorInvalidUsernameOrPassword);

                    if (result.Result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, ErrorAccountLockedOut);
                    }

                    return this.View(model);
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, ErrorInvalidUsernameOrPassword);
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
            var model = new ForgotPasswordFormModel
            {
                Email = "test@mail.com",
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                // return this.View(model);
                return Json(new { isValid = false, html = await this.RenderRazorViewToString("ForgotPassword", model) });
            }

            var result = await this.userService.GeneratePasswordResetTokenAsync(model.Email);

            if (!result.Succeeded)
            {
                foreach (var error in result.Messages)
                {
                    this.ModelState.AddModelError(string.Empty, error);
                }

                return this.View(model);
            }

            string endpointUri = Url.Action(nameof(ResetPassword), null, null)!;
            string passwordResetUrl =
                QueryHelpers.AddQueryString(endpointUri, nameof(ResetPasswordFormModel.Token), result.Data);

            var forgotPasswordViewModel = new ForgotPasswordMailViewModel
            {
                ResetPasswordUrl = passwordResetUrl,
                ApplicationName = GlobalConstants.SystemName,
            };

            var mailContent = await viewRenderer
                .RenderAsync(forgotPasswordViewModel, "ForgotPasswordEmailTemplate", GlobalConstants.MailTemplatePath);

            logger.LogInformation(mailContent);
            await emailService.SendResetPasswordEmail(model.Email, mailContent);
            return this.RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string? token, string? email)
        {
            if (token == null || email == null)
            {
                return this.BadRequest();
            }

            /*var model = new ForgotPasswordFormModel
                {
                    Token = token,
                    Email = email,
                };

                return this.View(model);*/

            return this.View();
        }
    }
}
