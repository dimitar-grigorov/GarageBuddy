namespace GarageBuddy.Services.Data.Models.Vehicle.BrandModel
{
    using AutoMapper;

    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandModelListServiceModel : BaseListServiceModel, IMapFrom<BrandModel>, IHaveCustomMappings
    {
        public Guid Id { get; init; }

        public string ModelName { get; set; } = null!;

        public Guid BrandId { get; set; }

        public string BrandName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<BrandModel, BrandModelListServiceModel>()
                .ForMember(dest => dest.BrandName, opt
                    => opt.MapFrom(src => src.Brand.BrandName));
        }
    }
}
