namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataTables.AspNet.AspNetCore;
    using DataTables.AspNet.Core;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Common;
    using Services.Data.Contracts;
    using Services.Data.Models.Vehicle.Brand;

    using ViewModels.Admin.Brand;

    public class BrandController : AdminController
    {
        private readonly IBrandService brandService;
        private readonly IMapper mapper;

        public BrandController(
            IBrandService brandService,
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

        public async Task<IActionResult> BrandList(IDataTablesRequest request)
        {
            if (request.Length <= 0)
            {
                return new DataTablesJsonResult(DataTablesResponse.Create(request, 0, 0, null), true);
            }

            request.AdditionalParameters.TryGetValue(IncludeDeletedFilterName, out var searchValue);

            var brandsResult = await this.brandService.GetAllAsync(
                new QueryOptions<BrandServiceModel>()
                {
                    IncludeDeleted = (bool)(searchValue ?? false),
                    Skip = request.Start,
                    Take = request.Length,
                });

            var data = mapper.Map<ICollection<BrandListViewModel>>(brandsResult.Data);

            /*var filteredData = String.IsNullOrWhiteSpace(request.Search.Value)
               ? data
               : data.Where(item => item.BrandName.Contains(request.Search.Value));*/

            var response = DataTablesResponse.Create(request, brandsResult.TotalCount, brandsResult.TotalCount, data);

            return new DataTablesJsonResult(response, true);
        }
    }
}
