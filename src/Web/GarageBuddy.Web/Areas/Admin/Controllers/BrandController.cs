namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataTables.AspNet.AspNetCore;
    using DataTables.AspNet.Core;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;

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

        /// <summary>
        /// This is your data method.
        /// DataTables will query this (HTTP GET) to fetch data to display.
        /// </summary>
        /// <param name="request">
        /// This represents your DataTables request.
        /// It's automatically binded using the default binder and settings.
        ///
        /// You should use IDataTablesRequest as your model, to avoid unexpected behavior and allow
        /// custom binders to be attached whenever necessary.
        /// </param>
        /// <returns>
        /// Return data here, with a json-compatible result.
        /// </returns>
        public async Task<IActionResult> BrandList(IDataTablesRequest request)
        {
            var brands = await this.brandService.GetAllAsync();

            var data = mapper.Map<ICollection<BrandListViewModel>>(brands);

            // Global filtering.
            // Filter is being manually applied due to in-memmory (IEnumerable) data.
            // If you want something rather easier, check IEnumerableExtensions Sample.
            var filteredData = String.IsNullOrWhiteSpace(request.Search.Value)
                ? data
                : data.Where(item => item.BrandName.Contains(request.Search.Value));

            // Paging filtered data.
            // Paging is rather manual due to in-memmory (IEnumerable) data.
            var dataPage = filteredData.Skip(request.Start).Take(request.Length);

            // Response creation. To create your response you need to reference your request, to avoid
            // request/response tampering and to ensure response will be correctly created.
            var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            // Easier way is to return a new 'DataTablesJsonResult', which will automatically convert your
            // response to a json-compatible content, so DataTables can read it when received.
            return new DataTablesJsonResult(response, true);
        }
    }
}
