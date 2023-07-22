namespace GarageBuddy.Data.Models.Job
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Xml.Linq;

    using Enums;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.JobStatus;

    public class JobStatus : BaseDeletableModel<int>
    {
        protected JobStatus() // used by EF Core
        {
        }

        private JobStatus(JobStatusEnum @enum)
        {
            Id = (int)@enum;
            StatusName = @enum.ToString();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public sealed override int Id { get; set; }

        [Required]
        [MaxLength(JobStatusNameMaxLength)]
        public string StatusName { get; set; } = null!;

        public static implicit operator JobStatus(JobStatusEnum @enum) => new(@enum);

        public static implicit operator JobStatusEnum(JobStatus jobStatus) => (JobStatusEnum)jobStatus.Id;
    }
}
