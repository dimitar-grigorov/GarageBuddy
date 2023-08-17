namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Models;

    public class GarageService : BaseService<Garage, Guid>, IGarageService
    {
        private readonly IDeletableEntityRepository<Garage, Guid> garageRepository;

        public GarageService(
            IDeletableEntityRepository<Garage, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.garageRepository = entityRepository;
        }

        public async Task<ICollection<GarageServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.Deleted)
        {
            return await base.GetAllAsync<GarageServiceModel>(asReadOnly, includeDeleted);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await garageRepository.ExistsAsync(id);
        }

        public async Task<IResult<GarageServiceModel>> GetAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return await Result<GarageServiceModel>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Garage)));
            }

            var model = await base.GetAsync<GarageServiceModel>(id);
            return await Result<GarageServiceModel>.SuccessAsync(model);
        }

        public async Task<IResult<Guid>> CreateAsync(GarageServiceModel model)
        {
            var isValid = ValidateModel(model);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Garage)));
            }

            if (!model.IsDeleted && await AtLeastOneActiveGarageExistsAsync(Guid.Empty))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.NoMoreThanOneActiveGarage));
            }

            var garage = this.Mapper.Map<Garage>(model);

            var entity = await garageRepository.AddAsync(garage);
            await garageRepository.SaveChangesAsync();
            var id = entity?.Entity.Id ?? Guid.Empty;

            if (entity?.Entity.Id != Guid.Empty)
            {
                return await Result<Guid>.SuccessAsync(id);
            }

            return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotCreated, nameof(Garage)));
        }

        public async Task<IResult> EditAsync(Guid id, GarageServiceModel model)
        {
            if (!await ExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.EntityNotFound, nameof(GarageServiceModel)));
            }

            if (!model.IsDeleted && await AtLeastOneActiveGarageExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.NoMoreThanOneActiveGarage));
            }

            var isValid = base.ValidateModel(model);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(GarageServiceModel)));
            }

            await base.EditAsync(id, model);

            return await Result<Guid>.SuccessAsync();
        }

        public async Task<bool> AtLeastOneActiveGarageExistsAsync(Guid excludeId)
        {
            return await garageRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted)
                .AnyAsync(g => g.Id.ToString() != excludeId.ToString());
        }

        public async Task<IResult<string>> GetCoordinatesOfTheActiveGarageAsync()
        {
            var garage = await garageRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted)
                .FirstOrDefaultAsync();
            if (garage == null)
            {
                return await Result<string>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Garage)));
            }

            var coordinates = garage.Coordinates;
            if (string.IsNullOrEmpty(coordinates))
            {
                return await Result<string>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Garage)));
            }

            return await Result<string>.SuccessAsync(data: coordinates);
        }
    }
}
