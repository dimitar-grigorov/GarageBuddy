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
                .ThenBy(v => v.Brand.BrandName)
                .ThenBy(v => v.BrandModel.ModelName)
                .ThenBy(v => v.Customer.Name)
                .Select(v => new VehicleSelectServiceModel
                {
                    Id = v.Id.ToString(),
                    BrandName = v.Brand.BrandName,
                    ModelName = v.BrandModel.ModelName,
                    CustomerName = v.Customer.Name,
                }).ToListAsync();
        }
    }
}
