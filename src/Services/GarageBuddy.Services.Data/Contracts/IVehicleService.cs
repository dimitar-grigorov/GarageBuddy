namespace GarageBuddy.Services.Data.Contracts
{
    using GarageBuddy.Services.Data.Models.Customer;
    using Models.Vehicle.Vehicle;

    public interface IVehicleService
    {
        public Task<ICollection<VehicleSelectServiceModel>> GetAllSelectAsync();
    }
}
