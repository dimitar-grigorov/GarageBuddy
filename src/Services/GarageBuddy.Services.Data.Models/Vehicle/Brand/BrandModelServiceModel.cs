﻿namespace GarageBuddy.Services.Data.Models.Vehicle.Brand
{
    using System;

    public class BrandModelServiceModel
    {
        Guid Id { get; set; }

        public string ModelName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public Guid BrandId { get; set; }

        public string BrandName { get; set; } = null!;
    }
}