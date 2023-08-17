namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataTables.AspNet.AspNetCore;
    using DataTables.AspNet.Core;
    using GarageBuddy.Common.Core.Enums;

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
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> BrandList(IDataTablesRequest request)
        {
            if (request.Length <= 0)
            {
                return new DataTablesJsonResult(DataTablesResponse.Create(request, 0, 0, null), true);
            }

            request.AdditionalParameters.TryGetValue(IncludeDeletedFilterName, out var includeDeleted);

            var brandsResult = await this.brandService.GetAllAsync(
                new QueryOptions<BrandServiceModel>()
                {
                    IncludeDeleted = (bool)(includeDeleted ?? false) ? DeletedFilter.Deleted : DeletedFilter.NotDeleted,
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
        public async Task<IActionResult> Create(BrandCreateOrEditViewModel model)
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

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Vehicle Brand");
            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!await this.brandService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Vehicle Brand"), (int)HttpStatusCode.NotFound);
            }

            var brand = await this.brandService.GetAsync(id);
            var model = mapper.Map<BrandCreateOrEditViewModel>(brand.Data);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, BrandCreateOrEditViewModel model)
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

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyEditedEntity, "Vehicle Brand");
            return RedirectToAction(Actions.Index);
        }
    }
}
