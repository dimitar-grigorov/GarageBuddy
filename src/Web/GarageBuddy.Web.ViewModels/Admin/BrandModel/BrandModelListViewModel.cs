namespace GarageBuddy.Web.ViewModels.Admin.BrandModel
{
    using System;

    using AutoMapper;

    using GarageBuddy.Services.Mapping.TypeConverters;

    using Services.Data.Models.Vehicle.BrandModel;
    using Services.Mapping;

    public class BrandModelListViewModel : IMapFrom<BrandModelListServiceModel>, IMapTo<BrandModelListServiceModel>, IHaveCustomMappings
    {

        public Guid Id { get; init; }

        public string ModelName { get; set; } = null!;

        public string CreatedOn { get; set; } = null!;

        public string? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public string? DeletedOn { get; set; }

        public Guid BrandId { get; set; }

        public string BrandName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
