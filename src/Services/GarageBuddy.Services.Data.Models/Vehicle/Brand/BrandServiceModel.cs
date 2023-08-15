namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using System;

    using AutoMapper;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandServiceModel : IMapFrom<Brand>, IMapTo<Brand>, IMapTo<BrandSelectServiceModel>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string BrandName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<BrandServiceModel, Brand>()
                .ForMember(d => d.Id,
                    opt => opt.Ignore());
        }
    }
}
