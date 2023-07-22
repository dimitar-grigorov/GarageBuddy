namespace GarageBuddy.Data.Models.Job
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using GarageBuddy.Data.Common.Models;

    using Microsoft.EntityFrameworkCore;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.JobItemPart;
    using static GarageBuddy.Common.Constants.GlobalValidationConstants;

    public class JobItemPart : BaseDeletableModel<Guid>
    {
        public Guid JobItemId { get; set; }

        [Required]
        [MaxLength(PartNameMaxLength)]
        public string PartName { get; set; } = null!;

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal Quantity { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal Price { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? CostPrice { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal TotalPrice { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; }

        public DateTime? WarrantyValidUntil { get; set; }

        // Navigation properties
        [ForeignKey(nameof(JobItemId))]
        public JobItem JobItem { get; set; } = null!;
    }
}
