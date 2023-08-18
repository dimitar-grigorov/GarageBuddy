namespace GarageBuddy.Web.ViewModels.Admin.Brand
{
    using Services.Data.Models.Vehicle.Brand;
    using Services.Mapping.TypeConverters;

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
        [Sanitize]
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
