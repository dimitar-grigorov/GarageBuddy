namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    /// <summary>
    /// Used to return the count of models by brand.
    /// </summary>
    public class ModelCountByBrandServiceModel
    {
        public string BrandName { get; set; }

        public int ModelCount { get; set; }
    }
}
