namespace GarageBuddy.Services.Data.Models.Vehicle.DriveType
{
    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class DriveTypeServiceModel : BaseListServiceModel, IMapFrom<DriveType>, IMapTo<DriveType>
    {
        public int Id { get; set; }

        public string DriveTypeName { get; set; } = null!;
    }
}
