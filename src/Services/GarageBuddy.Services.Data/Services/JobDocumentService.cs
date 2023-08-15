namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Job;

    public class JobDocumentService : BaseService<JobDocument, Guid>, IJobDocumentService
    {
        private readonly IDeletableEntityRepository<JobDocument, Guid> jobDocumentRepository;

        public JobDocumentService(
            IDeletableEntityRepository<JobDocument, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.jobDocumentRepository = entityRepository;
        }
    }
}
