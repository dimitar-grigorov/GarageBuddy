namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

    public class JobItemTypeController : AdminController
    {
        private readonly IJobItemTypeService jobItemTypeService;

        public JobItemTypeController(
            IJobItemTypeService jobItemTypeService)
        {
            this.jobItemTypeService = jobItemTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await this.jobItemTypeService.GetAllAsync();
            return this.View(model);
        }
    }
}
