namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class BrandSelectServiceModel : IMapFrom<Brand>
    {
        public string Id { get; set; } = null!;

        public string BrandName { get; set; } = null!;
    }
}
