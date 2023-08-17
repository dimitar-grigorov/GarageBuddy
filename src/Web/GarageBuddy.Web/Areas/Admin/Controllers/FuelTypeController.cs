namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.Vehicle.FuelType;

    using ViewModels.Admin.FuelType;

    public class FuelTypeController : AdminController
    {
        private readonly IFuelTypeService fuelTypeService;

        private readonly IMapper mapper;

        public FuelTypeController(
            IFuelTypeService fuelTypeService,
            IMapper mapper)
        {
            this.fuelTypeService = fuelTypeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.fuelTypeService.GetAllAsync();
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FuelTypeCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brandServiceModel = mapper.Map<FuelTypeServiceModel>(model);
            var result = await this.fuelTypeService.CreateAsync(brandServiceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Fuel type");
            return RedirectToAction(Actions.Index);
        }
    }
}
