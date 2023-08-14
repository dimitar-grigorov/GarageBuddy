namespace GarageBuddy.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Services.Data.Common;

    using Models.Vehicle.Brand;

    public interface IBrandService
    {
        public Task<ICollection<BrandServiceModel>> GetAllAsync(bool asReadOnly = false, bool includeDeleted = false);

        public Task<PaginatedResult<BrandServiceModel>> GetAllAsync(QueryOptions<BrandServiceModel> queryOptions);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<BrandServiceModel>> GetAsync(Guid id);

        public Task<bool> BrandNameExistsAsync(string brandName);

        public Task<IResult<Guid>> CreateAsync(BrandServiceModel brandServiceModel);

        public Task<IResult> EditAsync(Guid id, BrandServiceModel brandServiceModel);
    }
}
