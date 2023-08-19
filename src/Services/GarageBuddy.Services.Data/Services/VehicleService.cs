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

        private readonly ICustomerService customerService;
        private readonly IBrandService brandService;
        private readonly IBrandModelService brandModelService;
        private readonly IFuelTypeService fuelTypeService;
        private readonly IGearboxTypeService gearboxTypeService;
        private readonly IDriveTypeService driveTypeService;

        public VehicleService(
            IDeletableEntityRepository<Vehicle, Guid> entityRepository,
            ICustomerService customerService,
            IBrandService brandService,
            IBrandModelService brandModelService,
            IFuelTypeService fuelTypeService,
            IGearboxTypeService gearboxTypeService,
            IDriveTypeService driveTypeService,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.vehicleRepository = entityRepository;

            this.customerService = customerService;
            this.brandService = brandService;
            this.brandModelService = brandModelService;
            this.fuelTypeService = fuelTypeService;
            this.gearboxTypeService = gearboxTypeService;
            this.driveTypeService = driveTypeService;
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
            if (!ValidateModel(model))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Vehicle)));
            }

            var relationValidation = await ValidateRelationsAsync(model);
            if (!relationValidation.Succeeded)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityRelationsAreNotValid, nameof(Vehicle)));
            }

            var entityModel = this.Mapper.Map<Vehicle>(model);

            if (entityModel.BrandModelId == Guid.Empty)
            {
                entityModel.BrandModelId = null;
            }

            if (entityModel.DateOfManufacture is { Year: < 2000 })
            {
                entityModel.DateOfManufacture = null;
            }

            var entity = await vehicleRepository.AddAsync(entityModel);
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
            if (!await ExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.EntityNotFound, nameof(Vehicle)));
            }

            if (model.BrandModelId == Guid.Empty)
            {
                model.BrandModelId = null;
            }

            var relationValidation = await ValidateRelationsAsync(model);
            if (!relationValidation.Succeeded)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityRelationsAreNotValid, nameof(Vehicle)));
            }

            return await base.EditAsync(id, model, nameof(Vehicle));
        }

        /// <inheritdoc/>
        public async Task<IResult> ValidateRelationsAsync(VehicleServiceModel model)
        {
            var result = new Result();

            if (!await customerService.ExistsAsync(model.CustomerId))
            {
                result.Messages.Add(nameof(model.CustomerId));
            }

            if (!await brandService.ExistsAsync(model.BrandId))
            {
                result.Messages.Add(nameof(model.BrandId));
            }

            if (model.BrandModelId != null &&
                !await brandModelService.ExistsAsync(model.BrandModelId.Value))
            {
                result.Messages.Add(nameof(model.BrandModelId));
            }

            if (model.FuelTypeId != null && !await fuelTypeService.ExistsAsync(model.FuelTypeId.Value))
            {
                result.Messages.Add(nameof(model.FuelTypeId));
            }

            if (model.GearboxTypeId != null && !await gearboxTypeService.ExistsAsync(model.GearboxTypeId.Value))
            {
                result.Messages.Add(nameof(model.GearboxTypeId));
            }

            if (model.DriveTypeId != null && !await driveTypeService.ExistsAsync(model.DriveTypeId.Value))
            {
                result.Messages.Add(nameof(model.DriveTypeId));
            }

            result.Succeeded = !result.Messages.Any();
            return result;
        }
    }
}
