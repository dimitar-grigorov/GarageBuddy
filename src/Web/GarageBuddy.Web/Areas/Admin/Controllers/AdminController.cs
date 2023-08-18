namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Common.Constants;

    using GarageBuddy.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Admin")]
    public class AdminController : BaseController
    {
        public AdminController(IHtmlSanitizer sanitizer) : base(sanitizer)
        {
            // Nothing to do here
        }
    }
}
