namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

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

        public async Task<ICollection<BrandServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.NotDeleted)
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
            return await brandRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
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
            return await brandRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
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

        public async Task<ICollection<ModelCountByBrandServiceModel>> GetModelCountByBrandAsync(int brandsLimit, bool shuffledData)
        {
            var brandData = await brandRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted)
                .Include(b => b.BrandModels)
                .OrderByDescending(b => b.BrandModels.Count())
                .Take(brandsLimit)
                .Select(b => new ModelCountByBrandServiceModel
                {
                    BrandName = b.BrandName,
                    ModelCount = b.BrandModels.Count(),
                }).ToListAsync();

            if (shuffledData)
            {
                var random = new Random();
                int n = brandData.Count;
                while (n > 1)
                {
                    n--;
                    var k = random.Next(n + 1);
                    (brandData[k], brandData[n]) = (brandData[n], brandData[k]);
                }
            }
            return brandData;
        }
    }
}
