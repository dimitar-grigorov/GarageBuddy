namespace GarageBuddy.Services.Data.Models.Job.JobStatus
{
    using Base;

    using GarageBuddy.Data.Models.Job;

    using Mapping;

    public class JobStatusServiceModel : BaseListServiceModel, IMapFrom<JobStatus>, IMapTo<JobStatus>
    {
        public int Id { get; set; }

        public string StatusName { get; set; } = null!;
    }
}
