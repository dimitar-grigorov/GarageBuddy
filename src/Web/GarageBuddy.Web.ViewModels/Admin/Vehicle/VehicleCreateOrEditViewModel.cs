namespace GarageBuddy.Web.ViewModels.Admin.Vehicle
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Base;

    using Services.Data.Models.Customer;
    using Services.Data.Models.Vehicle.Brand;
    using Services.Data.Models.Vehicle.BrandModel;
    using Services.Data.Models.Vehicle.DriveType;
    using Services.Data.Models.Vehicle.FuelType;
    using Services.Data.Models.Vehicle.GearboxType;
    using Services.Data.Models.Vehicle.Vehicle;
    using Services.Mapping;
    using Services.Mapping.TypeConverters;

    using static Common.Constants.EntityValidationConstants.Vehicle;
    using static Common.Constants.GlobalValidationConstants;

    public class VehicleCreateOrEditViewModel : BaseCreateOrEditViewModel,
        IMapTo<VehicleServiceModel>,
        IMapFrom<VehicleServiceModel>,
        IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public Guid CustomerId { get; set; }

        public IEnumerable<CustomerSelectServiceModel> Customer { get; set; }
            = new List<CustomerSelectServiceModel>();

        [Required]
        [Display(Name = "Brand")]
        public Guid BrandId { get; set; }

        public IEnumerable<BrandSelectServiceModel> Brand { get; set; }
            = new List<BrandSelectServiceModel>();

        [Display(Name = "Model")]
        public Guid? BrandModelId { get; set; }

        public IEnumerable<BrandModelSelectServiceModel> BrandModel { get; set; }
            = new List<BrandModelSelectServiceModel>();

        [Display(Name = "VIN")]
        [StringLength(VehicleVinNumberMaxLength)]
        public string? VehicleIdentificationNumber { get; set; }

        [Display(Name = "Registration Number")]
        [StringLength(VehicleRegistrationNumberMaxLength)]
        public string? RegistrationNumber { get; set; }

        //[DataType(DataType.Date)]
        [Display(Name = "Date of Manufacture")]
        public string? DateOfManufacture { get; set; }

        [Display(Name = "Fuel Type")]
        public int? FuelTypeId { get; set; }

        public IEnumerable<FuelTypeSelectServiceModel> FuelType { get; set; }
            = new List<FuelTypeSelectServiceModel>();

        [Display(Name = "Gearbox Type")]
        public int? GearboxTypeId { get; set; }

        public IEnumerable<GearboxTypeSelectServiceModel> GearboxType { get; set; }
            = new List<GearboxTypeSelectServiceModel>();

        [Display(Name = "Drive Type")]
        public int? DriveTypeId { get; set; }

        public IEnumerable<DriveTypeSelectServiceModel> DriveType { get; set; }
            = new List<DriveTypeSelectServiceModel>();

        [Display(Name = "Engine Capacity")]
        public int? EngineCapacity { get; set; }

        [Display(Name = "Engine Horse Power")]
        public int? EngineHorsePower { get; set; }

        [Display(Name = "Engine Torque")]
        [StringLength(DefaultDescriptionMaxLength)]
        public string? Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<DateTime?, string?>().ConvertUsing(new ReverseDateTimeTypeConverter());
        }
    }
}
