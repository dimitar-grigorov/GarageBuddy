namespace GarageBuddy.Web.ViewModels.Admin.Brand
{
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Services.Data.Models.Vehicle.Brand;
    using GarageBuddy.Services.Mapping;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Brand;

    public class BrandCreateViewModel : IMapFrom<BrandServiceModel>, IMapTo<BrandServiceModel>
    {

        [Required]
        [StringLength(BrandNameMaxLength, MinimumLength = BrandNameMinLength)]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; } = null!;
    }
}
