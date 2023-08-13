namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using ViewModels.Admin.Brand;

    public class BrandController : AdminController
    {

        private readonly IBrandService brandService;
        private readonly IMapper mapper;

        public BrandController(IBrandService brandService, 
            IMapper mapper)
        {
            this.brandService = brandService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var brands = await this.brandService.GetAllAsync();

            var model = mapper.Map<ICollection<BrandListViewModel>>(brands);

            return View(model);
        }
    }
}
