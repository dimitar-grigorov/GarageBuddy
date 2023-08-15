namespace GarageBuddy.Services.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Common;

    using Contracts;

    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.BrandModel;

    public class BrandModelService : BaseService<BrandModel, Guid>, IBrandModelService
    {
        private readonly IDeletableEntityRepository<BrandModel, Guid> brandModelRepository;
        private readonly IBrandService brandService;

        public BrandModelService(
            IDeletableEntityRepository<BrandModel, Guid> brandModelRepository,
            IMapper mapper,
            IBrandService brandService)
            : base(brandModelRepository, mapper)
        {
            this.brandModelRepository = brandModelRepository;
            this.brandService = brandService;
        }

        public async Task<ICollection<BrandModelListServiceModel>> GetAllAsync(bool asReadOnly = false, bool includeDeleted = false)
        {
            var query = this.brandModelRepository
                .All(asReadOnly, includeDeleted)
                .Include(bm => bm.Brand)
                .ProjectTo<BrandModelListServiceModel>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return await query;
        }

        public async Task<PaginatedResult<BrandModelListServiceModel>> GetAllAsync(QueryOptions<BrandModelListServiceModel> queryOptions)
        {
            // Default order by name
            if (!queryOptions.OrderOptions.Any())
            {
                queryOptions.OrderOptions = new List<OrderOption<BrandModelListServiceModel>>
                        {
                            new(e => e.ModelName, OrderByOrder.Ascending),
                            new(e => e.BrandName, OrderByOrder.Ascending),
                        };
            }

            var query = this.brandModelRepository
                .All(queryOptions.AsReadOnly, queryOptions.IncludeDeleted)
                .Include(bm => bm.Brand)
                .ProjectTo<BrandModelListServiceModel>(Mapper.ConfigurationProvider);

            var modelList = await ModifyQuery(query, queryOptions).ToListAsync();

            var totalCount = await GetTotalCountForPagination(queryOptions);

            return PaginatedResult<BrandModelListServiceModel>.Success(modelList, totalCount);
        }

        public async Task<PaginatedResult<BrandModelListServiceModel>> GetAllByBrandIdAsync(Guid brandId,
            QueryOptions<BrandModelListServiceModel> queryOptions)
        {
            // Default order by name
            if (!queryOptions.OrderOptions.Any())
            {
                queryOptions.OrderOptions = new List<OrderOption<BrandModelListServiceModel>>
                {
                    new(e => e.ModelName, OrderByOrder.Ascending),
                    new(e => e.BrandName, OrderByOrder.Ascending),
                };
            }

            var query = this.brandModelRepository
                .All(queryOptions.AsReadOnly, queryOptions.IncludeDeleted)
                .Where(bm => bm.BrandId == brandId)
                .Include(bm => bm.Brand)
                .ProjectTo<BrandModelListServiceModel>(Mapper.ConfigurationProvider);

            var modelList = await ModifyQuery(query, queryOptions).ToListAsync();

            var totalCount = await GetTotalCountForPagination(queryOptions);

            return PaginatedResult<BrandModelListServiceModel>.Success(modelList, totalCount);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await brandModelRepository.ExistsAsync(id);
        }

        public async Task<IResult<BrandModelServiceModel>> GetAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return await Result<BrandModelServiceModel>.FailAsync(string.Format(Errors.EntityNotFound, nameof(BrandModel)));
            }

            var model = await base.GetAsync<BrandModelServiceModel>(id);
            return await Result<BrandModelServiceModel>.SuccessAsync(model);
        }

        public async Task<IResult<Guid>> CreateAsync(BrandModelServiceModel model)
        {
            var isValid = base.ValidateModel(model);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(BrandModel)));
            }

            if (!await this.brandService.ExistsAsync(model.BrandId))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            // TODO: Check if works
            var brandModel = this.Mapper.Map<BrandModel>(model);

            var entity = await brandModelRepository.AddAsync(brandModel);
            await brandModelRepository.SaveChangesAsync();
            var id = entity?.Entity.Id ?? Guid.Empty;

            if (entity?.Entity.Id != Guid.Empty)
            {
                return await Result<Guid>.SuccessAsync(id);
            }

            return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotCreated, nameof(BrandModel)));
        }

        public async Task<IResult> EditAsync(Guid id, BrandModelServiceModel model)
        {
            if (!await ExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.EntityNotFound, nameof(BrandModel)));
            }

            var isValid = base.ValidateModel(model);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            if (!await this.brandService.ExistsAsync(model.BrandId))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            var brand = await brandModelRepository.FindAsync(id, false);

            // TODO: check if works
            this.Mapper.Map(model, brand);

            brandModelRepository.Update(brand);
            await brandModelRepository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync();
        }
    }
}
