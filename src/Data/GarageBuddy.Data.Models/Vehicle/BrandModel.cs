namespace GarageBuddy.Data.Models.Vehicle
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using GarageBuddy.Data.Common.Models;

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

        [Required]
        public bool IsSeeded { get; set; } = false;
    }
}
