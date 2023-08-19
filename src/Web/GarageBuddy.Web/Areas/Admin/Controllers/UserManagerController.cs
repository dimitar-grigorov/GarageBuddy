namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Net;

    using Common.Constants;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.ApplicationUser;

    public class UserManagerController : AdminController
    {
        private readonly IUserService userService;

        public UserManagerController(
            IHtmlSanitizer sanitizer,
            IUserService userService)
            : base(sanitizer)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var usersAndRoles = await this.userService.GetAllUsersWithRolesAsync();
            ViewData[GlobalConstants.AllRolesKey] = await this.userService.GetAllRolesAsync();

            return View(usersAndRoles);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserServiceModel model)
        {
            if (!await this.userService.ExistsAsync(model.Id.ToString() ?? string.Empty))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "User"), (int)HttpStatusCode.NotFound);
            }

            if (!ModelState.IsValid)
            {
                TempData[NotifyError] = string.Format(Errors.EntityModelStateIsNotValid, "User");
                return RedirectToAction(nameof(Index));
            }

            this.SanitizeModel(model);
            await this.userService.EditAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
