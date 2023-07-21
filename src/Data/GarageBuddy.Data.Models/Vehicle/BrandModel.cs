namespace GarageBuddy.Data.Models.Vehicle
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.BrandModel;

    public class BrandModel : BaseDeletableModel<Guid>
    {
        [Required]
        [MaxLength(BrandModelNameMaxLength)]
        public string ModelName { get; set; } = null!;
    }
}
