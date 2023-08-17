namespace GarageBuddy.Web.ViewModels.Admin.FuelType
{
    using System.ComponentModel.DataAnnotations;

    using Base;

    using GarageBuddy.Services.Data.Models.Vehicle.FuelType;
    using Services.Mapping;

    using static Common.Constants.EntityValidationConstants.FuelType;

    public class FuelTypeCreateOrEditViewModel :
        BaseCreateOrEditViewModel,
        IMapFrom<FuelTypeServiceModel>,
        IMapTo<FuelTypeServiceModel>
    {
        [Required]
        [StringLength(FuelTypeNameMaxLength, MinimumLength = FuelTypeNameMinLength)]
        [Display(Name = "Fuel name")]
        public string FuelName { get; set; } = null!;
    }
}
