namespace GarageBuddy.Web.ViewModels.Admin.BrandModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Base;

    using Services.Data.Models.Vehicle.Brand;
    using Services.Data.Models.Vehicle.BrandModel;
    using Services.Mapping;
    using Services.Mapping.TypeConverters;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.BrandModel;

    public class BrandModelCreateOrEditViewModel :
        BaseCreateOrEditViewModel,
        IMapTo<BrandModelServiceModel>,
        IMapFrom<BrandModelServiceModel>,
        IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        [StringLength(BrandModelNameMaxLength, MinimumLength = BrandModelNameMinLength)]
        [Sanitize]
        public string ModelName { get; set; } = null!;

        [Required]
        [Display(Name = "Brand")]
        public Guid BrandId { get; set; }

        public IEnumerable<BrandSelectServiceModel> Brands { get; set; }
            = new List<BrandSelectServiceModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
