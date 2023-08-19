namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Common.Constants;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

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
    }
}
