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
                .OrderBy(b => b.IsDeleted)
                .ThenBy(b => b.Brand.BrandName)
                .ThenBy(b => b.BrandModel.ModelName)
                .ThenBy(b => b.Customer.Name)
                .Select(b => new VehicleSelectServiceModel
                {
                    Id = b.Id.ToString(),
                    BrandName = b.Brand.BrandName,
                    ModelName = b.BrandModel.ModelName,
                    CustomerName = b.Customer.Name,
                }).ToListAsync();
        }
    }
}
