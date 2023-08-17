namespace GarageBuddy.Data.Models.Vehicle
{
    using static GarageBuddy.Common.Constants.EntityValidationConstants.FuelType;

    public class FuelType : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(FuelTypeNameMaxLength)]
        public string FuelName { get; set; } = null!;
    }
}
