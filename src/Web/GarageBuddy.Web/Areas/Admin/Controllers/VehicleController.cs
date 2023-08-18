namespace GarageBuddy.Web.Areas.Admin.Controllers
{
    using AutoMapper;

    using GarageBuddy.Services.Data.Contracts;

    using Microsoft.AspNetCore.Mvc;

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
            var model = await this.vehicleService.GetAllAsync();

            return this.View();
        }
    }
}
