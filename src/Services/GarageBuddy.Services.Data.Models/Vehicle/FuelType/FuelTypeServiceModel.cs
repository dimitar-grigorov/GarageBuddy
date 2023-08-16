namespace GarageBuddy.Services.Data.Models.Vehicle.FuelType
{
    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class FuelTypeServiceModel : BaseListServiceModel, IMapFrom<FuelType>, IMapTo<FuelType>
    {
        public int Id { get; set; }

        public string FuelName { get; set; } = null!;
    }
}
