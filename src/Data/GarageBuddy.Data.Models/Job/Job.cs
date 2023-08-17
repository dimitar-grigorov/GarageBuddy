namespace GarageBuddy.Data.Models.Job
{
    using System.Collections.Generic;

    using GarageBuddy.Data.Models.Vehicle;

    using Microsoft.EntityFrameworkCore;

    using static GarageBuddy.Common.Constants.GlobalValidationConstants;

    public class Job : BaseDeletableModel<Guid>
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid VehicleId { get; set; }

        [Required]
        public int JobStatusId { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? EstimatedPrice { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? TotalDiscount { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? TotalPrice { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? TotalPaid { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? KilometersDriven { get; set; }

        public DateTime? StartedOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        public DateTime? CompletedOnEstimated { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CustomerId))]
        public virtual Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(VehicleId))]
        public virtual Vehicle Vehicle { get; set; } = null!;

        [ForeignKey(nameof(JobStatusId))]
        public virtual JobStatus JobStatus { get; set; } = null!;

        public virtual ICollection<JobItem> JobItems { get; set; }
            = new HashSet<JobItem>();

        public virtual ICollection<JobDocument> JobDocuments { get; set; }
            = new HashSet<JobDocument>();
    }
}
