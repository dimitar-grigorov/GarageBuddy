namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

    [Authorize(Policy = Policies.ManagerPolicy)]
    public class JobItemTypeController : AdminController
    {
        private readonly IJobItemTypeService jobItemTypeService;

        public JobItemTypeController(
            IHtmlSanitizer sanitizer,
            IJobItemTypeService jobItemTypeService)
            : base(sanitizer)
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
