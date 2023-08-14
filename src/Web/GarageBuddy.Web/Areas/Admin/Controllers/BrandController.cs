namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brandServiceModel = mapper.Map<BrandServiceModel>(model);
            var result = await this.brandService.CreateAsync(brandServiceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!await this.brandService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Brand"), (int)HttpStatusCode.NotFound);
            }

            var brand = await this.brandService.GetAsync(id);
            var model = mapper.Map<BrandViewModel>(brand.Data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, BrandViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var brandServiceModel = mapper.Map<BrandServiceModel>(model);
            var result = await this.brandService.EditAsync(id, brandServiceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
            }

            return RedirectToAction(Actions.Index);
        }
    }
}
