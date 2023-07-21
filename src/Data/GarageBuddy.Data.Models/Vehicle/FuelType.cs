namespace GarageBuddy.Data.Models.Vehicle
{
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.FuelType;

    public class FuelType : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(FuelTypeNameMaxLength)]
        public string FuelName { get; set; } = null!;
    }
}
