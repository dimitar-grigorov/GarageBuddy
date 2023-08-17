namespace GarageBuddy.Web.ViewModels.Admin.DriveType
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using GarageBuddy.Services.Mapping;

    using Services.Data.Models.Vehicle.DriveType;

    using static Common.Constants.EntityValidationConstants.DriveType;

    public class DriveTypeCreateOrEditViewModel : 
        BaseCreateOrEditViewModel,
        IMapFrom<DriveTypeServiceModel>, IMapTo<DriveTypeServiceModel>
    {
        [Required]
        [StringLength(DriveTypeNameMaxLength, MinimumLength = DriveTypeNameMinLength)]
        [Display(Name = "Drive type name")]
        public string DriveTypeName { get; set; } = null!;
    }
}