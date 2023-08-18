namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Services.Data.Models.Vehicle.Brand;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.BrandModel;

    public class BrandModelService : BaseService<BrandModel, Guid>, IBrandModelService
    {
        private readonly IDeletableEntityRepository<BrandModel, Guid> brandModelRepository;
        private readonly IBrandService brandService;

        public BrandModelService(
            IDeletableEntityRepository<BrandModel, Guid> entityRepository,
            IMapper mapper,
            IBrandService brandService)
            : base(entityRepository, mapper)
        {
            this.brandModelRepository = entityRepository;
            this.brandService = brandService;
        }

        public async Task<ICollection<BrandModelListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted)
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

        public async Task<ICollection<BrandSelectServiceModel>> GetAllSelectAsync(Guid brandId)
        {
            return await brandModelRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .OrderBy(b => b.IsDeleted)
                .ThenBy(b => b.ModelName)
                .Include(b => b.Brand)
                .Where(b => b.BrandId == brandId)
                .Select(b => new BrandSelectServiceModel
                {
                    Id = b.Id.ToString(),
                    BrandName = b.ModelName,
                })
                .ToListAsync();
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
            if (!await brandService.ExistsAsync(model.BrandId))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            return await CreateBasicAsync(model, "Brand model");
        }

        public async Task<IResult> EditAsync(Guid id, BrandModelServiceModel model)
        {
            if (!await this.brandService.ExistsAsync(model.BrandId))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Brand)));
            }

            return await base.EditAsync(id, model, "Brand Model");
        }
    }
}
