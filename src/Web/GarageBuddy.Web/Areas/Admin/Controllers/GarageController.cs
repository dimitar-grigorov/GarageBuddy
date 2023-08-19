namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using AutoMapper;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models;

    using ViewModels.Admin.Garage;

    [Authorize(Policy = Policies.ManagerPolicy)]
    public class GarageController : AdminController
    {
        private readonly IGarageService garageService;

        private readonly IMapper mapper;

        public GarageController(
            IHtmlSanitizer sanitizer,
            IGarageService garageService,
            IMapper mapper) : base(sanitizer)
        {
            this.garageService = garageService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var serviceModel = await this.garageService.GetAllAsync();
            var model = mapper.Map<ICollection<GarageListViewModel>>(serviceModel);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GarageCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SanitizeModel(model);
            var serviceModel = mapper.Map<GarageServiceModel>(model);
            var result = await this.garageService.CreateAsync(serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Garage");
            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var serviceModel = await this.garageService.GetAsync(id);
            var model = mapper.Map<GarageCreateOrEditViewModel>(serviceModel.Data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, GarageCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SanitizeModel(model);
            var serviceModel = mapper.Map<GarageServiceModel>(model);
            var result = await this.garageService.EditAsync(id, serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyEditedEntity, "Garage");
            return RedirectToAction(Actions.Index);
        }
    }
}
