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
        public int Id { get; set; }

        [Required]
        [StringLength(FuelTypeNameMaxLength, MinimumLength = FuelTypeNameMinLength)]
        [Display(Name = "Fuel name")]
        [Sanitize]
        public string FuelName { get; set; } = null!;
    }
}
