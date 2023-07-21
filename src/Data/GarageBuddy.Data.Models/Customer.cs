namespace GarageBuddy.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Customer;
    using static GarageBuddy.Common.Constants.GlobalValidationConstants;

    public class Customer : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(CustomerNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DefaultAddressMaxLength)]
        public string? Address { get; set; }

        [MaxLength(DefaultEmailMaxLength)]
        public string? Email { get; set; }

        [MaxLength(CustomerPhoneMaxLength)]
        public string? Phone { get; set; }

        [MaxLength(CustomerCompanyNameMaxLength)]
        public string? CompanyName { get; set; }

        [MaxLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; } = null!;

        public Guid? ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
