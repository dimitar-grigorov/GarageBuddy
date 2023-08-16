namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Data.Contracts;

    public class DriveTypeController : AdminController
    {

        private readonly IDriveTypeService driveTypeService;

        public DriveTypeController(
            IDriveTypeService driveTypeService)
        {
            this.driveTypeService = driveTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.driveTypeService.GetAllAsync();
            return this.View(model);
        }
    }
}
