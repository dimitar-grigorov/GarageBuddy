namespace GarageBuddy.Services.Data.Models.Job.JobItemType
{
    using Base;

    using GarageBuddy.Data.Models.Job;

    using Mapping;

    public class JobItemTypeServiceModel : BaseListServiceModel, IMapFrom<JobItemType>, IMapTo<JobItemType>
    {
        public string Id { get; set; } = null!;

        public string JobTypeName { get; set; } = null!;
    }
}
