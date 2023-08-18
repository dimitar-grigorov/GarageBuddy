namespace GarageBuddy.Services.Data.Models.Vehicle.GearboxType
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.GearboxType;

    public class GearboxTypeServiceModel :
        BaseListServiceModel,
        IMapFrom<GearboxType>,
        IMapTo<GearboxType>,
        IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(GearboxTypeNameMaxLength, MinimumLength = GearboxTypeNameMinLength)]
        public string GearboxTypeName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<GearboxTypeServiceModel, GearboxType>()
                .ForMember(d => d.Id,
                    opt => opt.Ignore());
        }
    }
}
