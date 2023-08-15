namespace GarageBuddy.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Common;

    using Contracts;

    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

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
            // Default order by name
            if (!queryOptions.OrderOptions.Any())
            {
                queryOptions.OrderOptions = new List<OrderOption<BrandServiceModel>>
                {
                    new(e => e.BrandName, OrderByOrder.Ascending),
                };
            }

            return await base.GetAllAsync(queryOptions);
        }

        public async Task<ICollection<BrandSelectServiceModel>> GetAllSelectAsync()
        {
            return await brandRepository.All(true, true)
                .OrderBy(b => b.IsDeleted)
                .ThenBy(b => b.BrandName)
                .Select(b => new BrandSelectServiceModel
                {
                    Id = b.Id.ToString(),
                    BrandName = b.BrandName,
                }).ToListAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await brandRepository.ExistsAsync(id);
        }

        public async Task<IResult<BrandServiceModel>> GetAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return await Result<BrandServiceModel>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            var model = await base.GetAsync<BrandServiceModel>(id);
            return await Result<BrandServiceModel>.SuccessAsync(model);
        }

        public async Task<bool> BrandNameExistsAsync(string brandName, Guid excludeId)
        {
            return await brandRepository.All(true, true)
                .AnyAsync(b => b.BrandName == brandName && b.Id.ToString() != excludeId.ToString());
        }

        public async Task<IResult<Guid>> CreateAsync(BrandServiceModel brandServiceModel)
        {
            var isValid = base.ValidateModel(brandServiceModel);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            if (await this.BrandNameExistsAsync(brandServiceModel.BrandName, Guid.Empty))
            {
                return await Result<Guid>.FailAsync(string.Format(string.Format(
                        Errors.EntityWithTheSameNameAlreadyExists, nameof(Brand), brandServiceModel.BrandName)));
            }

            var brand = this.Mapper.Map<Brand>(brandServiceModel);

            var entity = await brandRepository.AddAsync(brand);
            await brandRepository.SaveChangesAsync();
            var id = entity?.Entity.Id ?? Guid.Empty;

            if (entity?.Entity.Id != Guid.Empty)
            {
                return await Result<Guid>.SuccessAsync(id);
            }

            return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotCreated, nameof(Brand)));
        }

        public async Task<IResult> EditAsync(Guid id, BrandServiceModel brandServiceModel)
        {
            if (!await ExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            if (await this.BrandNameExistsAsync(brandServiceModel.BrandName, id))
            {
                return await Result<Guid>.FailAsync(string.Format(string.Format(
                    Errors.EntityWithTheSameNameAlreadyExists, nameof(Brand), brandServiceModel.BrandName)));
            }

            var isValid = base.ValidateModel(brandServiceModel);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            await base.EditAsync(id, brandServiceModel);

            return await Result<Guid>.SuccessAsync();
        }
    }
}
