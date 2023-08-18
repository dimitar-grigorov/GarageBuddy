namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using GarageBuddy.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    public class DashboardController : AdminController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
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
