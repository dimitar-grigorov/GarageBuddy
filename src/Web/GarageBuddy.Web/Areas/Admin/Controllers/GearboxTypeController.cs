namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.Vehicle.GearboxType;

    using ViewModels.Admin.GearboxType;

    [Authorize(Policy = Policies.ManagerPolicy)]
    public class GearboxTypeController : AdminController
    {
        private readonly IGearboxTypeService gearboxTypeService;

        private readonly IMapper mapper;

        public GearboxTypeController(
            IHtmlSanitizer sanitizer,
            IGearboxTypeService gearboxTypeService,
            IMapper mapper) : base(sanitizer)
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

            SanitizeModel(model);
            var serviceModel = mapper.Map<GearboxTypeServiceModel>(model);
            var result = await this.gearboxTypeService.CreateAsync(serviceModel);

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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.gearboxTypeService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Gearbox"), (int)HttpStatusCode.NotFound);
            }

            var gearboxResult = await this.gearboxTypeService.GetAsync(id);
            var model = mapper.Map<GearboxTypeCreateOrEditViewModel>(gearboxResult.Data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GearboxTypeCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SanitizeModel(model);
            var serviceModel = mapper.Map<GearboxTypeServiceModel>(model);
            var result = await this.gearboxTypeService.EditAsync(id, serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyEditedEntity, "Gearbox");
            return RedirectToAction(Actions.Index);
        }
    }
}
