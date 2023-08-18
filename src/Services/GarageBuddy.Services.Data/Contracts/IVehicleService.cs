namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.Vehicle;

    public interface IVehicleService
    {
        public Task<ICollection<VehicleSelectServiceModel>> GetAllSelectAsync();

        public Task<ICollection<VehicleListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.Deleted);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<VehicleServiceModel>> GetAsync(Guid id);

        public Task<IResult<Guid>> CreateAsync(VehicleServiceModel model);

        public Task<IResult> EditAsync(Guid id, VehicleServiceModel model);
    }
}
