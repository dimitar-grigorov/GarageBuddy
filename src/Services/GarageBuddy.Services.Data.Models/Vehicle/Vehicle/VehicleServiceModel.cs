namespace GarageBuddy.Services.Data.Models.Vehicle.Vehicle
{
    using System;

    using AutoMapper;

    using Base;

    using GarageBuddy.Data.Models.Vehicle;

    using Mapping;

    public class VehicleServiceModel : BaseListServiceModel,
        IMapFrom<Vehicle>, IMapTo<Vehicle>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public Guid BrandId { get; set; }

        public Guid? BrandModelId { get; set; }

        public string? VehicleIdentificationNumber { get; set; }

        public string? RegistrationNumber { get; set; }

        public DateOnly? DateOfManufacture { get; set; }

        public int? FuelTypeId { get; set; }

        public int? GearboxTypeId { get; set; }

        public int? DriveTypeId { get; set; }

        public int? EngineCapacity { get; set; }

        public int? EngineHorsePower { get; set; }

        public string? Description { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<VehicleServiceModel, Vehicle>()
                .ForMember(d => d.Id,
                    opt => opt.Ignore());

            configuration
                .CreateMap<Vehicle, VehicleServiceModel>()
                .ForMember(d => d.DateOfManufacture,
                                       opt => opt.MapFrom(s => s.DateOfManufacture ?? default));
        }
    }
}
