namespace GarageBuddy.Web.ViewModels.Admin.Brand
{
    using System;

    using Services.Data.Models.Vehicle.Brand;
    using Services.Mapping;

    public class BrandListViewModel : IMapFrom<BrandServiceModel>
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
