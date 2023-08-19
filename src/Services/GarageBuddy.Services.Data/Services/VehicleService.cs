namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using Contracts;

    using GarageBuddy.Common.Constants;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.Vehicle;

    public class VehicleService : BaseService<Vehicle, Guid>, IVehicleService
    {
        private readonly IDeletableEntityRepository<Vehicle, Guid> vehicleRepository;

        public VehicleService(
            IDeletableEntityRepository<Vehicle, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.vehicleRepository = entityRepository;
        }

        public async Task<ICollection<VehicleSelectServiceModel>> GetAllSelectAsync()
        {
            return await vehicleRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .Include(v => v.Brand)
                .Include(v => v.BrandModel)
                .OrderBy(v => v.IsDeleted)
                .Select(v => new VehicleSelectServiceModel
                {
                    Id = v.Id.ToString(),
                    BrandName = v.Brand.BrandName,
                    ModelName = v.BrandModel == null ? string.Empty : v.BrandModel.ModelName,
                    CustomerName = v.Customer.Name,
                })
                .OrderBy(v => v.BrandName)
                .ThenBy(v => v.ModelName)
                .ThenBy(v => v.CustomerName)
                .ToListAsync();
        }

        public async Task<ICollection<VehicleListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.Deleted)
        {
            var result = await vehicleRepository.All(asReadOnly, includeDeleted)
                .Include(v => v.Brand)
                .Include(v => v.BrandModel)
                .Include(v => v.Customer)
                .Select(v => new VehicleListServiceModel()
                {
                    Id = v.Id,
                    CreatedOn = v.CreatedOn,
                    ModifiedOn = v.ModifiedOn,
                    IsDeleted = v.IsDeleted,
                    DeletedOn = v.DeletedOn,
                    CustomerId = v.CustomerId,
                    CustomerName = v.Customer.Name,
                    BrandId = v.BrandId,
                    BrandName = v.Brand.BrandName,
                    BrandModelId = v.BrandModelId,
                    ModelName = v.BrandModel == null ? string.Empty : v.BrandModel.ModelName,
                    VehicleIdentificationNumber = v.VehicleIdentificationNumber,
                    RegistrationNumber = v.RegistrationNumber,
                    DateOfManufacture = v.DateOfManufacture == null ? string.Empty : v.DateOfManufacture.Value.ToString(GlobalConstants.DefaultDateFormat),
                    FuelTypeId = v.FuelTypeId,
                    GearboxTypeId = v.GearboxTypeId,
                    DriveTypeId = v.DriveTypeId,
                    EngineCapacity = v.EngineCapacity,
                    EngineHorsePower = v.EngineHorsePower,
                    Description = v.Description,
                }).OrderBy(v => v.FuelTypeId)
                .ThenBy(v => v.CustomerName)
                .ThenBy(v => v.RegistrationNumber)
                .ToListAsync();
            return result;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await vehicleRepository.ExistsAsync(id);
        }

        public async Task<IResult<VehicleServiceModel>> GetAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return await Result<VehicleServiceModel>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Vehicle)));
            }

            var model = await base.GetAsync<VehicleServiceModel>(id);
            return await Result<VehicleServiceModel>.SuccessAsync(model);
        }

        public async Task<IResult<Guid>> CreateAsync(VehicleServiceModel model)
        {
            var isValid = ValidateModel(model);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Vehicle)));
            }

            // TODO: check check check.
            var serviceModel = this.Mapper.Map<Vehicle>(model);

            if (serviceModel.BrandModelId == Guid.Empty)
            {
                serviceModel.BrandModelId = null;
            }

            if (serviceModel.DateOfManufacture is { Year: < 2000 })
            {
                serviceModel.DateOfManufacture = null;
            }

            var entity = await vehicleRepository.AddAsync(serviceModel);
            await vehicleRepository.SaveChangesAsync();
            var id = entity?.Entity.Id ?? Guid.Empty;

            if (entity?.Entity.Id != Guid.Empty)
            {
                return await Result<Guid>.SuccessAsync(id);
            }

            return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotCreated, nameof(Vehicle)));
        }

        public async Task<IResult> EditAsync(Guid id, VehicleServiceModel model)
        {
            throw new NotImplementedException();
        }
    }
}
