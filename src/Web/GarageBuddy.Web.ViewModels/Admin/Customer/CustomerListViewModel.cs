namespace GarageBuddy.Web.ViewModels.Admin.Customer
{
    using System;

    using Services.Data.Models.Customer;
    using Services.Mapping;

    public class CustomerListViewModel : IMapFrom<CustomerListServiceModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? CompanyName { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public string? UserName { get; set; }
    }
}
