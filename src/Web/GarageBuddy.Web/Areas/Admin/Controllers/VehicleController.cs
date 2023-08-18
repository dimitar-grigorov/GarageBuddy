namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using AutoMapper;

    using GarageBuddy.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Admin.Vehicle;

    public class VehicleController : AdminController
    {
        private readonly IVehicleService vehicleService;

        private readonly IMapper mapper;

        public VehicleController(
            IVehicleService vehicleService,
            IMapper mapper)
        {
            this.vehicleService = vehicleService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var serviceModel = await this.vehicleService.GetAllAsync();
            var model = mapper.Map<ICollection<VehicleListViewModel>>(serviceModel);

            return this.View(model);
        }
    }
}
