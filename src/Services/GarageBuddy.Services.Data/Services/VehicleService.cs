namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using Contracts;

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
                    DateOfManufacture = DateOnly.FromDateTime(v.DateOfManufacture ?? DateTime.MinValue),
                    FuelTypeId = v.FuelTypeId,
                    FuelTypeName = v.FuelType == null ? string.Empty : v.FuelType.FuelName,
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
            throw new NotImplementedException();
        }

        public async Task<IResult<VehicleServiceModel>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult<Guid>> CreateAsync(VehicleServiceModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> EditAsync(Guid id, VehicleServiceModel model)
        {
            throw new NotImplementedException();
        }
    }
}
