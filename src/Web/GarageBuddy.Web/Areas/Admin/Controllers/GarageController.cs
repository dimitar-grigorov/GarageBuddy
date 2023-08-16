namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Contracts;

    public class GarageController : AdminController
    {
        IGarageService garageService;

        public GarageController(IGarageService garageService)
        {
            this.garageService = garageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
