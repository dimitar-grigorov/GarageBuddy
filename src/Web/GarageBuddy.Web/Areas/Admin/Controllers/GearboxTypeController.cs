namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

    public class GearboxTypeController : AdminController
    {
        private readonly IGearboxTypeService gearboxTypeService;

        public GearboxTypeController(
            IGearboxTypeService gearboxTypeService)
        {
            this.gearboxTypeService = gearboxTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.gearboxTypeService.GetAllAsync();
            return this.View(model);
        }
    }
}
