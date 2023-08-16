namespace GarageBuddy.Services.Data.Models.Vehicle.FuelType
{
    using System.ComponentModel.DataAnnotations;
    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.FuelType;

    public class FuelTypeServiceModel : BaseListServiceModel, IMapFrom<FuelType>, IMapTo<FuelType>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(FuelTypeNameMaxLength, MinimumLength = FuelTypeNameMinLength)]
        public string FuelName { get; set; } = null!;
    }
}
