namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Job;

    public class JobItemPartService : BaseService<JobItemPart, Guid>, IJobItemPartService
    {
        private readonly IDeletableEntityRepository<JobItemPart, Guid> jobItemPartRepository;

        public JobItemPartService(
            IDeletableEntityRepository<JobItemPart, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.jobItemPartRepository = entityRepository;
        }
    }
}
