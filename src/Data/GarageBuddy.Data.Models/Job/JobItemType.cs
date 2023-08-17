namespace GarageBuddy.Data.Models.Job
{
    using static GarageBuddy.Common.Constants.EntityValidationConstants.JobItemType;

    public class JobItemType : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(JobTypeNameMaxLength)]
        public string JobTypeName { get; set; } = null!;
    }
}
