namespace GarageBuddy.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using GarageBuddy.Services.Data.Common;

    using Models.Vehicle.Brand;

    public interface IBrandService
    {
        public Task<ICollection<BrandServiceModel>> GetAllAsync(QueryOptions<BrandServiceModel>? queryOptions = null);
    }
}
