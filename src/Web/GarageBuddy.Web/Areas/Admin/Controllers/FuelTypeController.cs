namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

    public class FuelTypeController : AdminController
    {
        private readonly IFuelTypeService fuelTypeService;

        public FuelTypeController(
            IFuelTypeService fuelTypeService)
        {
            this.fuelTypeService = fuelTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.fuelTypeService.GetAllAsync();
            return this.View(model);
        }
    }
}
