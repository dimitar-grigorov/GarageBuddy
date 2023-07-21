namespace GarageBuddy.Data.Models.Vehicle
{
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.DriveType;

    public class DriveType : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DriveTypeNameMaxLength)]
        public string DriveTypeName { get; set; } = null!;
    }
}
