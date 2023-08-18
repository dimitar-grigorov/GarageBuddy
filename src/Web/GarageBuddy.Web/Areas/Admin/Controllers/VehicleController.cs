namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Net;

    using AutoMapper;

    using GarageBuddy.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Models.Vehicle.Vehicle;

    using ViewModels.Admin.Vehicle;

    public class VehicleController : AdminController
    {
        private readonly IVehicleService vehicleService;

        private readonly ICustomerService customerService;

        private readonly IBrandService brandService;

        private readonly IBrandModelService brandModelService;

        private readonly IFuelTypeService fuelTypeService;

        private readonly IGearboxTypeService gearboxTypeService;

        private readonly IDriveTypeService driveTypeService;

        private readonly IMapper mapper;

        public VehicleController(IMapper mapper,
            IVehicleService vehicleService,
            ICustomerService customerService,
            IBrandService brandService,
            IBrandModelService brandModelService,
            IFuelTypeService fuelTypeService,
            IGearboxTypeService gearboxTypeService,
            IDriveTypeService driveTypeService)
        {
            this.vehicleService = vehicleService;
            this.mapper = mapper;
            this.customerService = customerService;
            this.brandService = brandService;
            this.brandModelService = brandModelService;
            this.fuelTypeService = fuelTypeService;
            this.gearboxTypeService = gearboxTypeService;
            this.driveTypeService = driveTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var serviceModel = await this.vehicleService.GetAllAsync();
            var model = mapper.Map<ICollection<VehicleListViewModel>>(serviceModel);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new VehicleCreateOrEditViewModel();
            await this.PopulateSelectLists(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VehicleCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await this.PopulateSelectLists(model);
                return View(model);
            }

            var serviceModel = mapper.Map<VehicleServiceModel>(model);


            // TODO: check check check!
            var result = await this.vehicleService.CreateAsync(serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Vehicle");
            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!await this.vehicleService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Vehicle"), (int)HttpStatusCode.NotFound);
            }

            var serviceModelResult = await this.vehicleService.GetAsync(id);
            var model = mapper.Map<VehicleCreateOrEditViewModel>(serviceModelResult.Data);
            await this.PopulateSelectLists(model);

            return View(model);
        }

        [NonAction]
        private async Task PopulateSelectLists(VehicleCreateOrEditViewModel model)
        {
            model.Customer = await this.customerService.GetAllSelectAsync();
            model.Brand = await this.brandService.GetAllSelectAsync();
            model.FuelType = await this.fuelTypeService.GetAllSelectAsync();
            model.GearboxType = await this.gearboxTypeService.GetAllSelectAsync();
            model.DriveType = await this.driveTypeService.GetAllSelectAsync();
        }

        [HttpGet]
        public async Task<IActionResult> GetModelsByBrand(string brandId)
        {
            var modelsForBrand = await brandModelService.GetAllSelectAsync(Guid.Parse(brandId));

            return Json(modelsForBrand);
        }
    }
}
