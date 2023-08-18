namespace GarageBuddy.Services.Data.Services
{
    using System;

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
            if (!ValidateModel(model))
            {
                return await Result<int>.FailAsync(string.Format(Errors.EntityModelStateIsNotValid, "Vehicle drive type"));
            }

            var driveType = this.Mapper.Map<DriveType>(model);

            var entity = await driveTypeRepository.AddAsync(driveType);
            await driveTypeRepository.SaveChangesAsync();
            var id = entity?.Entity.Id ?? UnknownId;

            if (entity?.Entity.Id > 0)
            {
                return await Result<int>.SuccessAsync(id);
            }

            return await Result<int>.FailAsync(string.Format(Errors.EntityNotCreated, "Vehicle drive type"));
        }

        public async Task<IResult> EditAsync(int id, DriveTypeServiceModel model)
        {
            if (!await ExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.EntityNotFound, "Vehicle drive type"));
            }

            if (!ValidateModel(model))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityModelStateIsNotValid, "Vehicle drive type"));
            }

            await base.EditAsync(id, model);

            return await Result<Guid>.SuccessAsync();
        }
    }
}
