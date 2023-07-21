namespace GarageBuddy.Data.Models.Job
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using Microsoft.EntityFrameworkCore;

    using static GarageBuddy.Common.Constants.GlobalValidationConstants;

    public class JobItem : BaseDeletableModel<Guid>
    {
        [Required]
        public Guid JobId { get; set; }

        [Required]
        public Guid JobItemTypeId { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal Quantity { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal Price { get; set; }

        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal? Discount { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal CostPrice { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal PartsPrice { get; set; }

        [Required]
        [Precision(DefaultDecimalPrecision, DefaultDecimalScale)]
        public decimal TotalPrice { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; }

        public DateTime? WarrantyValidUntil { get; set; }

        // Navigation properties
        public Job Job { get; set; } = null!;

        public JobItemType JobItemType { get; set; } = null!;

        public ICollection<JobItemPart> JobItemParts { get; set; }
            = new HashSet<JobItemPart>();
    }
}
