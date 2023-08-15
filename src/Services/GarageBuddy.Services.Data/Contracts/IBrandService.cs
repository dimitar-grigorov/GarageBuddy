namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.Brand;

    public interface IBrandService
    {
        public Task<ICollection<BrandServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted);

        public Task<PaginatedResult<BrandServiceModel>> GetAllAsync(QueryOptions<BrandServiceModel> queryOptions);

        public Task<ICollection<BrandSelectServiceModel>> GetAllSelectAsync();

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<BrandServiceModel>> GetAsync(Guid id);

        public Task<bool> BrandNameExistsAsync(string brandName, Guid excludeId);

        public Task<IResult<Guid>> CreateAsync(BrandServiceModel brandServiceModel);

        public Task<IResult> EditAsync(Guid id, BrandServiceModel brandServiceModel);
    }
}
