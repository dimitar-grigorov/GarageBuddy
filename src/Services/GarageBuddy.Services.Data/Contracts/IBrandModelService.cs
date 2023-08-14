namespace GarageBuddy.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Services.Data.Common;
    using GarageBuddy.Services.Data.Models.Vehicle.Brand;

    public interface IBrandModelService
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
