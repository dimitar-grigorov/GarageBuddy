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

        /// <summary>
        /// Checks if the vehicle is related to any other entities and if so, returns an error message.
        /// Error messages are the property names that are invalid.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<IResult> ValidateRelationsAsync(VehicleServiceModel model);
    }
}
