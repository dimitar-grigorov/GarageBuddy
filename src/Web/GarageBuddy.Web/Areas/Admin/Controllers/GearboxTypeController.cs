namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.Vehicle.GearboxType;
    using ViewModels.Admin.GearboxType;

    public class GearboxTypeController : AdminController
    {
        private readonly IGearboxTypeService gearboxTypeService;

        private readonly IMapper mapper;

        public GearboxTypeController(
            IGearboxTypeService gearboxTypeService,
            IMapper mapper)
        {
            this.gearboxTypeService = gearboxTypeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.gearboxTypeService.GetAllAsync();
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GearboxTypeCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brandServiceModel = mapper.Map<GearboxTypeServiceModel>(model);
            var result = await this.gearboxTypeService.CreateAsync(brandServiceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Gearbox");
            return RedirectToAction(Actions.Index);
        }
    }
}
