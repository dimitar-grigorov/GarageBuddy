namespace GarageBuddy.Services.Data.Contracts
{
    using Models;

    public interface IGarageService
    {
        public Task<ICollection<GarageServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.Deleted);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<GarageServiceModel>> GetAsync(Guid id);

        public Task<IResult<Guid>> CreateAsync(GarageServiceModel model);

        public Task<IResult> EditAsync(Guid id, GarageServiceModel model);

        public Task<bool> AtLeastOneActiveGarageExistsAsync(Guid excludeId);

        public Task<IResult<string>> GetCoordinatesOfTheActiveGarageAsync();
    }
}
