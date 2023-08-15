namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;

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
    }
}
