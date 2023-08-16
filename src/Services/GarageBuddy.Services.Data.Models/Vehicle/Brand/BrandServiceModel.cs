namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using System;

    using AutoMapper;
    using Base;
    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandServiceModel : BaseListServiceModel, IMapFrom<Brand>, IMapTo<Brand>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string BrandName { get; set; } = null!;
        
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<BrandServiceModel, Brand>()
                .ForMember(d => d.Id,
                    opt => opt.Ignore());
        }
    }
}
