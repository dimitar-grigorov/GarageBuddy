namespace GarageBuddy.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Common;

    using Contracts;

    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Models.Vehicle.Brand;

    public class BrandService : BaseService<Brand, Guid>, IBrandService
    {
        private readonly IDeletableEntityRepository<Brand, Guid> brandRepository;

        public BrandService(
            IDeletableEntityRepository<Brand, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.brandRepository = entityRepository;
        }

        public async Task<ICollection<BrandServiceModel>> GetAllAsync(bool asReadOnly = false, bool includeDeleted = false)
        {
            return await base.GetAllAsync<BrandServiceModel>(asReadOnly, includeDeleted);
        }

        public async Task<PaginatedResult<BrandServiceModel>> GetAllAsync(QueryOptions<BrandServiceModel> queryOptions)
        {
            return await base.GetAllAsync(queryOptions);
        }
    }
}
