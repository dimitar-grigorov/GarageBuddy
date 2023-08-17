namespace GarageBuddy.Data.Models.Vehicle
{
    using System.Collections.Generic;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Brand;

    public class Brand : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(BrandNameMaxLength)]
        public string BrandName { get; set; } = null!;

        [Required]
        public IEnumerable<BrandModel> BrandModels { get; set; } = new HashSet<BrandModel>();
    }
}
