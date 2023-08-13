namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using System;
    using GarageBuddy.Data.Models.Vehicle;
    using Mapping;

    public class BrandServiceModel : IMapFrom<Brand>, IMapTo<Brand>
    {
        public string BrandId { get; set; } = null!;

        public string BrandName { get; set; } = null!;

        public bool IsSeeded { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
