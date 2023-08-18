namespace GarageBuddy.Web.ViewModels.Admin.Vehicle
{
    using System;

    using Services.Data.Models.Vehicle.Vehicle;
    using Services.Mapping;

    public class VehicleListViewModel : IMapFrom<VehicleListServiceModel>
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public string? VehicleIdentificationNumber { get; set; }

        public string? RegistrationNumber { get; set; }

        public string DateOfManufacture { get; set; } = string.Empty;

        public int? EngineHorsePower { get; set; }

        public string? Description { get; set; }

        public string? CustomerName { get; set; }

        public string BrandName { get; set; } = null!;

        public string? ModelName { get; set; }
    }
}
