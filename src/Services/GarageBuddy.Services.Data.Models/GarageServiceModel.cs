namespace GarageBuddy.Services.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Data.Models;
    using GarageBuddy.Services.Data.Models.Base;

    using Mapping;

    public class GarageServiceModel : BaseListServiceModel, IMapFrom<Garage>, IMapTo<Garage>
    {
        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ImageUrl { get; set; }

        public string? WorkingHours { get; set; }

        public string? Description { get; set; }

        public string? Coordinates { get; set; }
    }
}
