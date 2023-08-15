namespace GarageBuddy.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Services.Data.Common;

    using Models.Vehicle.BrandModel;

    public interface IBrandModelService
    {
        public Task<ICollection<BrandModelListServiceModel>> GetAllAsync(bool asReadOnly = false, bool includeDeleted = false);

        public Task<PaginatedResult<BrandModelListServiceModel>> GetAllAsync(QueryOptions<BrandModelListServiceModel> queryOptions);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<BrandModelServiceModel>> GetAsync(Guid id);

        public Task<IResult<Guid>> CreateAsync(BrandModelServiceModel model);

        public Task<IResult> EditAsync(Guid id, BrandModelServiceModel model);
    }
}
