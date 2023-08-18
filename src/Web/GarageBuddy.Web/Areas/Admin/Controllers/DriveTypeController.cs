namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.Vehicle.DriveType;

    using ViewModels.Admin.DriveType;

    public class DriveTypeController : AdminController
    {
        private readonly IDriveTypeService driveTypeService;

        private readonly IMapper mapper;

        public DriveTypeController(
            IHtmlSanitizer sanitizer,
            IDriveTypeService driveTypeService,
            IMapper mapper) : base(sanitizer)
        {
            this.driveTypeService = driveTypeService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.driveTypeService.GetAllAsync();
            return this.View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DriveTypeCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            SanitizeModel(model);
            var serviceModel = mapper.Map<DriveTypeServiceModel>(model);
            var result = await this.driveTypeService.CreateAsync(serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Vehicle drive type");
            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!await this.driveTypeService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Vehicle drive type"), (int)HttpStatusCode.NotFound);
            }

            var serviceModelResult = await this.driveTypeService.GetAsync(id);
            var model = mapper.Map<DriveTypeCreateOrEditViewModel>(serviceModelResult.Data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DriveTypeCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            SanitizeModel(model);
            var serviceModel = mapper.Map<DriveTypeServiceModel>(model);
            var result = await this.driveTypeService.EditAsync(id, serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyEditedEntity, "Vehicle drive type");
            return RedirectToAction(Actions.Index);
        }
    }
}
