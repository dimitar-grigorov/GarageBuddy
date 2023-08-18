namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using System.Net;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;

    using Services.Data.Contracts;
    using Services.Data.Models.Customer;

    using ViewModels.Admin.Customer;

    public class CustomerController : AdminController
    {
        private readonly IMapper mapper;

        private readonly ICustomerService customerService;

        private readonly IUserService userService;

        public CustomerController(
            IMapper mapper,
            ICustomerService customerService,
            IUserService userService)
        {
            this.mapper = mapper;
            this.customerService = customerService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var serviceModel = await this.customerService.GetAllAsync();
            var model = mapper.Map<ICollection<CustomerListViewModel>>(serviceModel);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CustomerCreateOrEditViewModel
            {
                Users = await this.userService.GetAllSelectAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Users = await this.userService.GetAllSelectAsync();
                return View(model);
            }

            var serviceModel = mapper.Map<CustomerServiceModel>(model);

            // If user is selected and it does not exist in the database
            if (model.ApplicationUserId != null && !await userService.ExistsAsync(serviceModel.ApplicationUserId!.Value))
            {
                ModelState.AddModelError(nameof(model.ApplicationUserId), Errors.EntityDoesNotExist);
                model.Users = await this.userService.GetAllSelectAsync();
                return View(model);
            }

            var result = await this.customerService.CreateAsync(serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
                return View(model);
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyCreatedEntity, "Customer");
            return RedirectToAction(Actions.Index);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!await this.customerService.ExistsAsync(id))
            {
                return ShowError(string.Format(Errors.EntityNotFound, "Customer"), (int)HttpStatusCode.NotFound);
            }

            var serviceModelResult = await this.customerService.GetAsync(id);
            var model = mapper.Map<CustomerCreateOrEditViewModel>(serviceModelResult.Data);
            model.Users = await this.userService.GetAllSelectAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, CustomerCreateOrEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var serviceModel = mapper.Map<CustomerServiceModel>(model);
            var result = await this.customerService.EditAsync(id, serviceModel);

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Messages);
                ModelState.AddModelError(string.Empty, errors);
                TempData[NotifyError] = errors;
            }

            TempData[NotifySuccess] = string.Format(Success.SuccessfullyEditedEntity, "Garage");
            return RedirectToAction(Actions.Index);
        }
    }
}
