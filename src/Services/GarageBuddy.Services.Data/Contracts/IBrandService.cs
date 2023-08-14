namespace GarageBuddy.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Services.Data.Common;

    using Models.Vehicle.Brand;

    public interface IBrandService
    {
        public Task<ICollection<BrandServiceModel>> GetAllAsync(bool asReadOnly = false, bool includeDeleted = false);

        public Task<PaginatedResult<BrandServiceModel>> GetAllAsync(QueryOptions<BrandServiceModel> queryOptions);
    }
}
