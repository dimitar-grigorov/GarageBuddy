namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdminController
    {
        public DashboardController(IHtmlSanitizer sanitizer) : base(sanitizer)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            // TODO: Remove
            return RedirectToAction("Index", "Vehicle");

            /*var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);*/
        }
    }
}
