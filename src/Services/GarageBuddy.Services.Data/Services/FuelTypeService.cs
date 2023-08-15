namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

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
    }
}
