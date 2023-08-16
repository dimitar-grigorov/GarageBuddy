namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.GearboxType;

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

        public async Task<ICollection<GearBoxTypeSelectServiceModel>> GetAllSelectAsync()
        {
            return await gearboxTypeRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .OrderBy(b => b.IsDeleted)
                .ThenBy(b => b.GearboxTypeName)
                .Select(b => new GearBoxTypeSelectServiceModel
                {
                    Id = b.Id,
                    GearboxTypeName = b.GearboxTypeName,
                }).ToListAsync();
        }
    }
}
