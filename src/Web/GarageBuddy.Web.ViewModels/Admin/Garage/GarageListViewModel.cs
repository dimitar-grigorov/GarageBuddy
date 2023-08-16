namespace GarageBuddy.Web.ViewModels.Admin.Garage
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using GarageBuddy.Services.Data.Models;
    using GarageBuddy.Services.Mapping;

    public class GarageListViewModel : IMapFrom<GarageServiceModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Address { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? ImageUrl { get; set; }

        public string? WorkingHours { get; set; }

        public string? Description { get; set; }

        public bool IsDeleted { get; set; } = false;

    }
}
