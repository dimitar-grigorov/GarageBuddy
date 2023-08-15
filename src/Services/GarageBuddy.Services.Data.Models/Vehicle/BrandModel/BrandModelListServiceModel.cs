namespace GarageBuddy.Services.Data.Models.Vehicle.BrandModel
{
    using AutoMapper;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandModelListServiceModel : IMapFrom<BrandModel>, IHaveCustomMappings
    {
        public Guid Id { get; init; }

        public string ModelName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

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
