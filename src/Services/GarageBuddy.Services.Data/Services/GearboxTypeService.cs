namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    public class GearboxTypeService : BaseService<GearboxType, int>, IGearboxTypeService
    {
        private readonly IDeletableEntityRepository<GearboxType, int> gearboxTypeRepository;

        public GearboxTypeService(
            IDeletableEntityRepository<GearboxType, int> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.gearboxTypeRepository = entityRepository;
        }
    }
}
