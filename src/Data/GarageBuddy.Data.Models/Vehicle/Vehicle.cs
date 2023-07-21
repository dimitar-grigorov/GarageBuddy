namespace GarageBuddy.Data.Models.Vehicle
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using GarageBuddy.Data.Common.Models;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Vehicle;
    using static GarageBuddy.Common.Constants.GlobalValidationConstants;

    public class Vehicle : BaseDeletableModel<Guid>
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        // TODO: Consider if its required
        public Guid? BrandModelId { get; set; }

        [MaxLength(VehicleVinNumberMaxLength)]
        public string? VehicleIdentificationNumber { get; set; }

        [MaxLength(VehicleRegistrationNumberMaxLength)]
        public string? RegistrationNumber { get; set; }

        [Column(TypeName = "Date")]
        public DateTime? DateOfManufacture { get; set; }

        public int? FuelTypeId { get; set; }

        public int? GearboxTypeId { get; set; }

        public int? DriveTypeId { get; set; }

        public int? EngineCapacity { get; set; }

        public int? EngineHorsePower { get; set; }

        [MaxLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; } = null!;

        [ForeignKey(nameof(BrandModelId))]
        public BrandModel BrandModel { get; set; } = null!;

        [ForeignKey(nameof(FuelTypeId))]
        public FuelType? FuelType { get; set; }

        [ForeignKey(nameof(GearboxTypeId))]
        public GearboxType? GearboxType { get; set; }

        [ForeignKey(nameof(DriveTypeId))]
        public DriveType? DriveType { get; set; }
    }
}