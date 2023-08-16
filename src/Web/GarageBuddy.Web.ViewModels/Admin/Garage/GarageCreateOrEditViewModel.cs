namespace GarageBuddy.Web.ViewModels.Admin.Garage
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using GarageBuddy.Services.Mapping.TypeConverters;

    using Services.Data.Models;
    using Services.Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Garage;
    using static GarageBuddy.Common.Constants.GlobalValidationConstants;
    using static GarageBuddy.Common.Constants.MessageConstants;

    public class GarageCreateOrEditViewModel : IMapTo<GarageServiceModel>,
        IMapFrom<GarageServiceModel>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(GarageNameMaxLength, MinimumLength = GarageNameMinLength)]
        [Display(Name = "Garage Name")]
        public string Name { get; set; } = null!;

        [StringLength(GarageAddressMaxLength, MinimumLength = GarageAddressMinLength)]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [StringLength(DefaultEmailMaxLength)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(GaragePhoneMaxLength)]
        [Phone]
        public string? Phone { get; set; }

        [StringLength(UrlMaxLength)]
        [Url]
        [Display(Name = "Image Url")]
        public string? ImageUrl { get; set; }

        [StringLength(GarageWorkingHoursMaxLength)]
        [Display(Name = "Working Hours")]
        public string? WorkingHours { get; set; }

        [StringLength(DefaultDescriptionMaxLength)]
        [Display(Name = "Garage description")]
        public string? Description { get; set; }

        [StringLength(GarageCoordinatesMaxLength)]
        [RegularExpression(CoordinatesRegex, ErrorMessage = Errors.InvalidCoordinates)]
        [Display(Name = "Garage location (coordinates)")]
        public string? Coordinates { get; set; }

        [Display(Name = "Deactivated")]
        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Deactivated On")]
        public string? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
