namespace GarageBuddy.Services.Data.Models.Vehicle.BrandModel
{
    using System;

    using AutoMapper;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandModelServiceModel : IMapFrom<BrandModel>, IMapTo<BrandModel>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string ModelName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public Guid BrandId { get; set; }

        public string BrandName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<BrandModelServiceModel, BrandModel>()
                .ForMember(d => d.Id,
                    opt => opt.Ignore());
        }
    }
}
