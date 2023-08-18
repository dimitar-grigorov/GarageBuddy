namespace GarageBuddy.Services.Data.Models.Vehicle.Vehicle
{
    public class VehicleListServiceModel : VehicleServiceModel
    {
        public string? CustomerName { get; set; }

        public string BrandName { get; set; } = null!;

        public string? ModelName { get; set; }

        public string? FuelTypeName { get; set; }
    }
}
