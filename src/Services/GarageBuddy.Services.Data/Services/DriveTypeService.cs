namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using GarageBuddy.Common.Core.Wrapper;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Services.Data.Contracts;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.DriveType;

    public class DriveTypeService : BaseService<DriveType, int>, IDriveTypeService
    {
        private readonly IDeletableEntityRepository<DriveType, int> driveTypeRepository;

        public DriveTypeService(
            IDeletableEntityRepository<DriveType, int> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.driveTypeRepository = entityRepository;
        }

        public async Task<ICollection<DriveTypeServiceModel>> GetAllAsync()
        {
            var query = this.driveTypeRepository
                .All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .ProjectTo<DriveTypeServiceModel>(this.Mapper.ConfigurationProvider)
                .OrderBy(d => d.IsDeleted)
                .ThenBy(b => b.Id);
            return await query.ToListAsync();
        }

        public async Task<ICollection<DriveTypeSelectServiceModel>> GetAllSelectAsync()
        {
            return await driveTypeRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .OrderBy(d => d.IsDeleted)
                .ThenBy(b => b.DriveTypeName)
                .Select(b => new DriveTypeSelectServiceModel
                {
                    Id = b.Id,
                    DriveTypeName = b.DriveTypeName,
                }).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await driveTypeRepository.ExistsAsync(id);
        }

        public async Task<IResult<DriveTypeServiceModel>> GetAsync(int id)
        {
            if (!await ExistsAsync(id))
            {
                return await Result<DriveTypeServiceModel>.FailAsync(string.Format(Errors.EntityNotFound, "Vehicle drive type"));
            }

            var model = await base.GetAsync<DriveTypeServiceModel>(id);
            return await Result<DriveTypeServiceModel>.SuccessAsync(model);
        }

        public async Task<IResult<int>> CreateAsync(DriveTypeServiceModel model)
        {
            return await this.CreateBasicAsync(model, "Vehicle drive type");
        }

        public async Task<IResult> EditAsync(int id, DriveTypeServiceModel model)
        {
            return await base.EditAsync(id, model, "Vehicle drive type");
        }
    }
}
