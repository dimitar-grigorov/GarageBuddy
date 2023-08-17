namespace GarageBuddy.Web.ViewModels.Admin.Brand
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Base;

    using GarageBuddy.Services.Mapping.TypeConverters;

    using Services.Data.Models.Vehicle.Brand;
    using Services.Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Brand;

    public class BrandCreateOrEditViewModel :
        BaseCreateOrEditViewModel,
        IMapFrom<BrandServiceModel>,
        IMapTo<BrandServiceModel>,
        IHaveCustomMappings
    {
        public Guid BrandId { get; set; }

        [Required]
        [StringLength(BrandNameMaxLength, MinimumLength = BrandNameMinLength)]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; } = null!;

        public void CreateMappings(IProfileExpression configuration)
        {
            /*  configuration.CreateMap<DateTime?, string>().ConvertUsing(
                src =>
                    src.HasValue ? src.Value.ToString(GlobalConstants.DefaultDateTimeFormat)
                        : MessageConstants.NotDeleted);*/

            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
