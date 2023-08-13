namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using System;

    using AutoMapper;

    using GarageBuddy.Data.Models.Vehicle;
    using Mapping;

    public class BrandServiceModel : IMapFrom<Brand>, IMapTo<Brand>, IHaveCustomMappings
    {
        public string BrandId { get; set; } = null!;

        public string BrandName { get; set; } = null!;

        public bool IsSeeded { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<Brand, BrandServiceModel>()
                .ForMember(d => d.BrandId, opt =>
                    opt.MapFrom(s => s.Id));
        }
    }
}
