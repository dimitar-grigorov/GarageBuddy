namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.Vehicle;

    public interface IVehicleService
    {
        public Task<ICollection<VehicleSelectServiceModel>> GetAllSelectAsync();
    }
}
