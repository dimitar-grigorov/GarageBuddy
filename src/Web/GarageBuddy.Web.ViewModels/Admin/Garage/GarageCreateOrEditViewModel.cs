namespace GarageBuddy.Web.ViewModels.Admin.Garage
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Base;

    using Services.Data.Models;
    using Services.Mapping;
    using Services.Mapping.TypeConverters;

    using static Common.Constants.GlobalValidationConstants;
    using static Common.Constants.MessageConstants;
    using static GarageBuddy.Common.Constants.EntityValidationConstants.Garage;

    public class GarageCreateOrEditViewModel : BaseCreateOrEditViewModel,
        IMapTo<GarageServiceModel>,
        IMapFrom<GarageServiceModel>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(GarageNameMaxLength, MinimumLength = GarageNameMinLength)]
        [Display(Name = "Garage Name")]
        [Sanitize]
        public string Name { get; set; } = null!;

        [StringLength(GarageAddressMaxLength, MinimumLength = GarageAddressMinLength)]
        [Display(Name = "Address")]
        [Sanitize]
        public string? Address { get; set; }

        [StringLength(DefaultEmailMaxLength)]
        [EmailAddress]
        [Display(Name = "Email")]
        [Sanitize]
        public string? Email { get; set; }

        [StringLength(GaragePhoneMaxLength)]
        [Phone]
        [Sanitize]
        public string? Phone { get; set; }

        [StringLength(UrlMaxLength)]
        [Url]
        [Display(Name = "Image Url")]
        [Sanitize]
        public string? ImageUrl { get; set; }

        [StringLength(GarageWorkingHoursMaxLength)]
        [Display(Name = "Working Hours")]
        [Sanitize]
        public string? WorkingHours { get; set; }

        [StringLength(DefaultDescriptionMaxLength)]
        [Display(Name = "Garage description")]
        [Sanitize]
        public string? Description { get; set; }

        [StringLength(GarageCoordinatesMaxLength)]
        [RegularExpression(CoordinatesRegex, ErrorMessage = Errors.InvalidCoordinates)]
        [Display(Name = "Garage location (coordinates)")]
        [Sanitize]
        public string? Coordinates { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
