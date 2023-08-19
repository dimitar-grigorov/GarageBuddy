namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using GarageBuddy.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area("Admin")]
    [Authorize(Policy = Policies.MechanicPolicy)]
    public class AdminController : BaseController
    {
        public AdminController(IHtmlSanitizer sanitizer) : base(sanitizer)
        {
            // Nothing to do here
        }
    }
}
