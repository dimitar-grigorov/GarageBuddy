namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

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

        public async Task<ICollection<GearBoxTypeServiceModel>> GetAllAsync()
        {
            var query = this.gearboxTypeRepository
                .All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .ProjectTo<GearBoxTypeServiceModel>(this.Mapper.ConfigurationProvider)
                .OrderBy(d => d.IsDeleted)
                .ThenBy(b => b.Id);

            return await query.ToListAsync();
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
