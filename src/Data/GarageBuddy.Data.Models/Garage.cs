namespace GarageBuddy.Data.Models
{
    using static GarageBuddy.Common.Constants.EntityValidationConstants.Garage;

    public class Garage : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(GarageNameMaxLength)]
        public string Name { get; set; } = null!;

        [MaxLength(DefaultAddressMaxLength)]
        public string? Address { get; set; }

        [MaxLength(DefaultEmailMaxLength)]
        public string? Email { get; set; }

        [MaxLength(GaragePhoneMaxLength)]
        public string? Phone { get; set; }

        [MaxLength(UrlMaxLength)]
        public string? ImageUrl { get; set; }

        [MaxLength(GarageWorkingHoursMaxLength)]
        public string? WorkingHours { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; }

        [MaxLength(GarageCoordinatesMaxLength)]
        public string? Coordinates { get; set; }
    }
}
