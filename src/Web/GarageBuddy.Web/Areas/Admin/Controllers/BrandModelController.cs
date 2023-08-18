namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using AutoMapper;

    using Common.Core.Enums;

    using DataTables.AspNet.AspNetCore;
    using DataTables.AspNet.Core;

    using GarageBuddy.Services.Data.Common;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.Vehicle.BrandModel;

    using ViewModels.Admin.BrandModel;

    public class BrandModelController : AdminController
    {
        private readonly IBrandModelService brandModelService;
        private readonly IBrandService brandService;
        private readonly IMapper mapper;

        public BrandModelController(
            IHtmlSanitizer sanitizer,
            IBrandModelService brandModelService,
            IBrandService brandService,
            IMapper mapper) : base(sanitizer)
        {
            this.brandModelService = brandModelService;
            this.mapper = mapper;
            this.brandService = brandService;
        }

        [HttpGet]
        public IActionResult Index(string? brandId)
        {
            if (brandId != null)
            {
                TempData[IdFilterName] = brandId;
            }

            return View();
        }

        public async Task<IActionResult> BrandModelList(IDataTablesRequest request)
        {
            if (request.Length <= 0)
            {
                return new DataTablesJsonResult(DataTablesResponse.Create(request, 0, 0, null), true);
            }

            request.AdditionalParameters.TryGetValue(IncludeDeletedFilterName, out var includeDeleted);
            request.AdditionalParameters.TryGetValue(IdFilterName, out var brandId);

            var queryOptions = new QueryOptions<BrandModelListServiceModel>()
            {
                IncludeDeleted = (bool)(includeDeleted ?? false) ? DeletedFilter.Deleted : DeletedFilter.NotDeleted,
                Skip = request.Start,
                Take = request.Length,
            };

            // Do we have passed search value?
            var brandsResult = (brandId != null) && (string)brandId != string.Empty
                ? await this.brandModelService.GetAllByBrandIdAsync(Guid.Parse(brandId.ToString()!), queryOptions)
                : await this.brandModelService.GetAllAsync(queryOptions);

            var data = mapper.Map<ICollection<BrandModelListViewModel>>(brandsResult.Data);

            var response = DataTablesResponse.Create(request, brandsResult.TotalCount, brandsResult.TotalCount, data);

            return new DataTablesJsonResult(response, true);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new BrandModelCreateOrEditViewModel()
            {
                Brands = await this.brandService.GetAllSelectAsync(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandModelCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Brands = await this.brandService.GetAllSelectAsync();
                return View(model);
            }
            SanitizeModel(model);
            var serviceModel = mapper.Map<BrandModelServiceModel>(model);
            var result = await this.brandModelService.CreateAsync(serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                model.Brands = await this.brandService.GetAllSelectAsync();
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Vehicle model");
            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!await this.brandModelService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Vehicle model"), (int)HttpStatusCode.NotFound);
            }

            var serviceModel = await this.brandModelService.GetAsync(id);
            var model = mapper.Map<BrandModelCreateOrEditViewModel>(serviceModel.Data);

            model.Brands = await this.brandService.GetAllSelectAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, BrandModelCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Brands = await this.brandService.GetAllSelectAsync();
                return View(model);
            }

            SanitizeModel(model);
            var serviceModel = mapper.Map<BrandModelServiceModel>(model);
            var result = await this.brandModelService.EditAsync(id, serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyEditedEntity, "Vehicle model");
            return RedirectToAction(Actions.Index);
        }
    }
}
