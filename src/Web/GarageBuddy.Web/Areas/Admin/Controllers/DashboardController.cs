namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using GarageBuddy.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Admin.Dashboard;

    public class DashboardController : AdminController
    {
        private readonly ISettingsService settingsService;

        public DashboardController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public IActionResult Index()
        {
            // TODO: Remove
            return RedirectToAction("Index", "Brand");

            var viewModel = new IndexViewModel { SettingsCount = this.settingsService.GetCount(), };
            return this.View(viewModel);
        }
    }
}
