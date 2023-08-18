namespace GarageBuddy.Web.ViewModels.Admin.GearboxType
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using GarageBuddy.Services.Data.Models.Vehicle.GearboxType;

    using Services.Mapping;

    using static Common.Constants.EntityValidationConstants.GearboxType;

    public class GearboxTypeCreateOrEditViewModel :
        BaseCreateOrEditViewModel,
        IMapFrom<GearboxTypeServiceModel>,
        IMapTo<GearboxTypeServiceModel>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GearboxTypeNameMaxLength, MinimumLength = GearboxTypeNameMinLength)]
        [Display(Name = "Gearbox type")]
        [Sanitize]
        public string GearboxTypeName { get; set; } = null!;
    }
}
