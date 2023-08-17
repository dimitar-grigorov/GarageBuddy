namespace GarageBuddy.Services.Data.Contracts
{
    using GarageBuddy.Services.Data.Models.Vehicle.BrandModel;

    using Models.Vehicle.FuelType;

    public interface IFuelTypeService
    {
        public Task<ICollection<FuelTypeServiceModel>> GetAllAsync();

        public Task<ICollection<FuelTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<IResult<int>> CreateAsync(FuelTypeServiceModel model);
    }
}
