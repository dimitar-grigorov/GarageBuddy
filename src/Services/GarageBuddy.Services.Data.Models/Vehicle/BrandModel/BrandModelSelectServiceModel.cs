namespace GarageBuddy.Services.Data.Models.Vehicle.BrandModel
{
    using System;

    public class BrandModelSelectServiceModel
    {
        public Guid Id { get; set; }

        public string ModelName { get; set; } = null!;
    }
}
