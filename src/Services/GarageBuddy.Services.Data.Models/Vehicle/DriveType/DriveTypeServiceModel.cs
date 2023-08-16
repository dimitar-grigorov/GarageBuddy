namespace GarageBuddy.Services.Data.Models.Vehicle.DriveType
{
    using System.ComponentModel.DataAnnotations;
    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.DriveType;

    public class DriveTypeServiceModel : BaseListServiceModel, IMapFrom<DriveType>, IMapTo<DriveType>
    {
        public int Id { get; set; }

        [Required]
        [StringLength(DriveTypeNameMaxLength, MinimumLength = DriveTypeNameMinLength)]
        public string DriveTypeName { get; set; } = null!;
    }
}
