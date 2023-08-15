namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using Contracts;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Job;

    public class JobService : BaseService<Job, Guid>, IJobService
    {
        private readonly IDeletableEntityRepository<Job, Guid> jobRepository;

        public JobService(
            IDeletableEntityRepository<Job, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.jobRepository = entityRepository;
        }
    }
}
