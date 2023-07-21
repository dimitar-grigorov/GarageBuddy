namespace GarageBuddy.Data.Models.Job
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.JobItemType;

    public class JobItemType : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(JobTypeNameMaxLength)]
        public string JobTypeName { get; set; } = null!;
    }
}
