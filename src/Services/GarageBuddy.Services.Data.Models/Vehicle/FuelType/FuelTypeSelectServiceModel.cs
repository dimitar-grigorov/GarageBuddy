namespace GarageBuddy.Services.Data.Models.Vehicle.FuelType
{
    using AutoMapper;

    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Services.Data.Models.Base;

    using Mapping;

    public class FuelTypeSelectServiceModel : BaseListServiceModel,
        IMapFrom<FuelType>, IMapTo<FuelType>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FuelName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<FuelTypeSelectServiceModel, FuelType>()
                .ForMember(d => d.Id,
                    opt => opt.Ignore());
        }
    }
}
