namespace GarageBuddy.Services.Data.Models.Vehicle.GearboxType
{
    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class GearBoxTypeServiceModel : BaseListServiceModel, IMapFrom<GearboxType>, IMapTo<GearboxType>
    {
        public int Id { get; set; }

        public string GearboxTypeName { get; set; } = null!;
    }
}
