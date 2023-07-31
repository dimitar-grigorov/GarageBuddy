namespace GarageBuddy.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

    using ViewModels.User;

    using static Common.Constants.ErrorMessageConstants;

    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
        public IActionResult ResetPassword()
        {
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            try
            {
                // var result = await this.userService.ResetPasswordAsync(model.Email);
                var token = await this.userService.GeneratePasswordResetTokenAsync(model.Email);

                /*if (!result.Succeeded)
                {
                    // TODO: Send email with token
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return this.View(model);
                }*/
            }
            catch (Exception)
            {
                this.ModelState.AddModelError(string.Empty, ErrorSomethingWentWrong);
                return this.View(model);
            }

            return this.RedirectToAction(nameof(Login));
        }
    }
}
