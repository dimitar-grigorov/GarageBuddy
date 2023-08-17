namespace GarageBuddy.Web.Areas.Admin.Controllers
{
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
            IDriveTypeService driveTypeService,
            IMapper mapper)
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

            var brandServiceModel = mapper.Map<DriveTypeServiceModel>(model);
            var result = await this.driveTypeService.CreateAsync(brandServiceModel);

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
    }
}
