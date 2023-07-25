namespace GarageBuddy.Data.Models.Vehicle
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Brand;

    public class Brand : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(BrandNameMaxLength)]
        public string BrandName { get; set; } = null!;

        [Required]
        public bool IsSeeded { get; set; } = false;

        [Required]
        public IEnumerable<BrandModel> BrandModels { get; set; } = new HashSet<BrandModel>();
    }
}
