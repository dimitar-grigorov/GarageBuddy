namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Job;

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
    }
}
