namespace GarageBuddy.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using Services.Data.Models.Vehicle.Brand;

    public class HomeViewModel
    {
        public ICollection<ModelCountByBrandServiceModel> ModelsCountByBrand { get; set; } = null!;

        public string ActiveGarageCoordinates { get; set; } = string.Empty;
    }
}
