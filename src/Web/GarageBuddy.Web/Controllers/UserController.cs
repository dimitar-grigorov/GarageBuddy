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
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            // TODO: Check if needed
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
            };

            return this.View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, ErrorInvalidUsernameOrPassword);
                return this.View(model);
            }

            try
            {
                var result = this.userService.LoginWithUsernameAsync(
                    model.Username, model.Password, model.RememberMe, false);

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

            return this.RedirectToAction("Index", "Home");
        }
    }
}
