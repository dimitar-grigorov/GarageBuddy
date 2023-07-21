namespace GarageBuddy.Data.Models.Job
{
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.JobStatus;

    public class JobStatus : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(JobStatusNameMaxLength)]
        public string StatusName { get; set; } = null!;
    }
}
