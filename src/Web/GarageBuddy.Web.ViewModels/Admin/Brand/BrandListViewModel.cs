namespace GarageBuddy.Web.ViewModels.Admin.Brand
{
    using System;

    using AutoMapper;

    using Services.Data.Models.Vehicle.Brand;
    using Services.Mapping;
    using Services.Mapping.TypeConverters;

    public class BrandListViewModel : IMapFrom<BrandServiceModel>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string BrandName { get; set; } = null!;

        public bool IsSeeded { get; set; }

        public string CreatedOn { get; set; } = null!;

        public string? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
