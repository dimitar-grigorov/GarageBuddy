namespace GarageBuddy.Data.Models.Job
{
    using static GarageBuddy.Common.Constants.EntityValidationConstants.JobDocument;

    public class JobDocument : BaseDeletableModel<Guid>
    {
        public Guid JobId { get; set; }

        [Required]
        [MaxLength(DocumentNameMaxLength)]
        public string DocumentName { get; set; } = null!;

        [Required]
        [MaxLength(UrlMaxLength)]
        public string DocumentUrl { get; set; } = null!;

        // Navigation properties
        public Job Job { get; set; } = null!;
    }
}
