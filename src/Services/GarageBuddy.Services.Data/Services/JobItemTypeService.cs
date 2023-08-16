namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Job;

    using Microsoft.EntityFrameworkCore;

    using Models.Job.JobItemType;

    public class JobItemTypeService : BaseService<JobItemType, Guid>, IJobItemTypeService
    {
        private readonly IDeletableEntityRepository<JobItemType, Guid> jobItemTypeRepository;

        public JobItemTypeService(
            IDeletableEntityRepository<JobItemType, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.jobItemTypeRepository = entityRepository;
        }

        public async Task<ICollection<JobItemTypeSelectServiceModel>> GetAllSelectAsync()
        {
            return await jobItemTypeRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .OrderBy(b => b.IsDeleted)
                .ThenBy(b => b.JobTypeName)
                .Select(b => new JobItemTypeSelectServiceModel
                {
                    Id = b.Id.ToString(),
                    JobTypeName = b.JobTypeName,
                }).ToListAsync();
        }
    }
}
