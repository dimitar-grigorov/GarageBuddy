namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.FuelType;

    public interface IFuelTypeService
    {
        public Task<ICollection<FuelTypeServiceModel>> GetAllAsync();

        public Task<ICollection<FuelTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<IResult<int>> CreateAsync(FuelTypeServiceModel model);

        public Task<IResult> EditAsync(int id, FuelTypeServiceModel model);
    }
}
