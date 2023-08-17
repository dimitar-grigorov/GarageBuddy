namespace GarageBuddy.Data.Models.Vehicle
{
    using static GarageBuddy.Common.Constants.EntityValidationConstants.BrandModel;

    public class BrandModel : BaseDeletableModel<Guid>
    {
        public Guid BrandId { get; set; }

        [Required]
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; } = null!;

        [Required]
        [MaxLength(BrandModelNameMaxLength)]
        public string ModelName { get; set; } = null!;
    }
}
