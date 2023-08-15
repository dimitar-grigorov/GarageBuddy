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

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Common.Core.Wrapper.Generic;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.BrandModel;

    public class BrandModelService : BaseService<BrandModel, Guid>, IBrandModelService
    {
        private readonly IDeletableEntityRepository<BrandModel, Guid> entityRepository;
        private readonly IBrandService brandService;

        public BrandModelService(
            IDeletableEntityRepository<BrandModel, Guid> entityRepository,
            IMapper mapper,
            IBrandService brandService)
            : base(entityRepository, mapper)
        {
            this.entityRepository = entityRepository;
            this.brandService = brandService;
        }

        public async Task<ICollection<BrandModelListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal, 
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted)
        {
            var query = this.entityRepository
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

            var query = this.entityRepository
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

            var query = this.entityRepository
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
            return await entityRepository.ExistsAsync(id);
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

            var brandModel = this.Mapper.Map<BrandModel>(model);

            var entity = await entityRepository.AddAsync(brandModel);
            await entityRepository.SaveChangesAsync();
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

            await base.EditAsync(id, model);

            return await Result<Guid>.SuccessAsync();
        }
    }
}
