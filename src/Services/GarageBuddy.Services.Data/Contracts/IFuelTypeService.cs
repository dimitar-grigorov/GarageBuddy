namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.FuelType;

    public interface IFuelTypeService
    {
        public Task<ICollection<FuelTypeSelectServiceModel>> GetAllSelectAsync();
    }
}
