namespace GarageBuddy.Web.ViewModels.Admin.Brand
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Common.Constants;

    using Services.Data.Models.Vehicle.Brand;
    using Services.Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Brand;

    public class BrandViewModel : IMapFrom<BrandServiceModel>, IMapTo<BrandServiceModel>, IHaveCustomMappings
    {
        public Guid BrandId { get; set; }

        [Required]
        [StringLength(BrandNameMaxLength, MinimumLength = BrandNameMinLength)]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; } = null!;

        [Required]
        [Display(Name = "Is Deleted")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted On")]
        public string? DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<BrandViewModel, BrandServiceModel>()
                .ForMember(d => d.DeletedOn,
                    opt => opt.Ignore());

            configuration
                .CreateMap<BrandViewModel, BrandServiceModel>()
                .ForMember(d => d.CreatedOn,
                    opt => opt.Ignore());

            configuration.CreateMap<DateTime?, string>().ConvertUsing(
                src =>
                    src.HasValue ? src.Value.ToString(GlobalConstants.DefaultDateTimeFormat)
                        : MessageConstants.NotDeleted);

            configuration.CreateMap<BrandServiceModel, BrandViewModel>()
                    .ForMember(dest =>
                        dest.DeletedOn, opts
                        => opts.MapFrom(src => src.DeletedOn));
        }
    }
}
