namespace GarageBuddy.Services.Data.Models.Vehicle.GearboxType
{
    using System.ComponentModel.DataAnnotations;
    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.GearboxType;

    public class GearBoxTypeServiceModel : BaseListServiceModel, IMapFrom<GearboxType>, IMapTo<GearboxType>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(GearboxTypeNameMaxLength, MinimumLength = GearboxTypeNameMinLength)]
        public string GearboxTypeName { get; set; } = null!;
    }
}
