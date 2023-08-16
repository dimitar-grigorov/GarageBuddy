namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.FuelType;

    public class FuelTypeService : BaseService<FuelType, int>, IFuelTypeService
    {
        private readonly IDeletableEntityRepository<FuelType, int> fuelTypeRepository;

        public FuelTypeService(
            IDeletableEntityRepository<FuelType, int> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.fuelTypeRepository = entityRepository;
        }

        public async Task<ICollection<FuelTypeSelectServiceModel>> GetAllSelectAsync()
        {
            return await fuelTypeRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .OrderBy(b => b.IsDeleted)
                .ThenBy(b => b.FuelName)
                .Select(b => new FuelTypeSelectServiceModel
                {
                    Id = b.Id,
                    FuelName = b.FuelName,
                }).ToListAsync();
        }
    }
}
