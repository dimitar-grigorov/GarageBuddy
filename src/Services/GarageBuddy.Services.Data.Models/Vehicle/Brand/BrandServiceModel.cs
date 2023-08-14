namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using System;

    using AutoMapper;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandServiceModel : IMapFrom<Brand>, IMapTo<Brand>, IHaveCustomMappings
    {
        public Guid BrandId { get; set; }

        public string BrandName { get; set; } = null!;

        public bool IsSeeded { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<BrandServiceModel, Brand>()
                .ForMember(d => d.DeletedOn,
                    opt => opt.Ignore());

            configuration
                .CreateMap<BrandServiceModel, Brand>()
                .ForMember(d => d.CreatedOn,
                    opt => opt.Ignore());

            configuration
                .CreateMap<Brand, BrandServiceModel>()
                .ForMember(d => d.BrandId, opt =>
                    opt.MapFrom(s => s.Id));
        }
    }
}
