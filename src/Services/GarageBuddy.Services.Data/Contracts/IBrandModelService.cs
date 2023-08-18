namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.BrandModel;

    public interface IBrandModelService
    {
        public Task<ICollection<BrandModelListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted);

        public Task<PaginatedResult<BrandModelListServiceModel>> GetAllAsync(
            QueryOptions<BrandModelListServiceModel> queryOptions);

        public Task<ICollection<BrandModelSelectServiceModel>> GetAllSelectAsync(Guid brandId);

        public Task<PaginatedResult<BrandModelListServiceModel>> GetAllByBrandIdAsync(
            Guid brandId, QueryOptions<BrandModelListServiceModel> queryOptions);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<BrandModelServiceModel>> GetAsync(Guid id);

        public Task<IResult<Guid>> CreateAsync(BrandModelServiceModel model);

        public Task<IResult> EditAsync(Guid id, BrandModelServiceModel model);
    }
}
