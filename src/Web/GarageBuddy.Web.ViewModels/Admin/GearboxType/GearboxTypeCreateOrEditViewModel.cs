namespace GarageBuddy.Web.ViewModels.Admin.GearboxType
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using GarageBuddy.Services.Data.Models.Vehicle.GearboxType;
    using GarageBuddy.Services.Mapping;

    using static Common.Constants.EntityValidationConstants.GearboxType;

    public class GearboxTypeCreateOrEditViewModel :
        BaseCreateOrEditViewModel,
        IMapFrom<GearBoxTypeServiceModel>,
        IMapTo<GearBoxTypeServiceModel>
    {
        [Required]
        [StringLength(GearboxTypeNameMaxLength, MinimumLength = GearboxTypeNameMinLength)]
        [Display(Name = "Gearbox type")]
        public string GearboxTypeName { get; set; } = null!;
    }
}