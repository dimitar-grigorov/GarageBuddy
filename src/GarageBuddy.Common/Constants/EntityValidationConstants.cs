namespace GarageBuddy.Common.Constants
{
    using System.ComponentModel;

    public static class EntityValidationConstants
    {
        public static class ApplicationUser
        {
            public const int UserNameMinLength = 5;
            public const int UserNameMaxLength = 60;

            public const int EmailMinLength = 5;
            public const int EmailMaxLength = 60;

            /*public const int PasswordMinLength = 5;
            public const int PasswordMaxLength = 50;*/

            public const int ResetPasswordTokenMinLength = 5;
            public const int ResetPasswordTokenMaxLength = 100;
        }

        public static class Job
        {
            public const decimal KilometersDrivenMinValue = 0;
            public const decimal KilometersDrivenMaxValue = 100_000_000;
        }

        public static class JobDocument
        {
            public const int DocumentNameMinLength = 2;
            public const int DocumentNameMaxLength = 100;
        }

        // public static class JobItem {}
        public static class JobItemPart
        {
            public const int PartNameMinLength = 3;
            public const int PartNameMaxLength = 120;
        }

        public static class JobItemType
        {
            public const int JobTypeNameMinLength = 3;
            public const int JobTypeNameMaxLength = 80;
        }

        public static class JobStatus
        {
            public const int JobStatusNameMinLength = 3;
            public const int JobStatusNameMaxLength = 30;
        }

        public static class Brand
        {
            public const int BrandNameMinLength = 2;
            public const int BrandNameMaxLength = 60;
        }

        public static class BrandModel
        {
            public const int BrandModelNameMinLength = 2;
            public const int BrandModelNameMaxLength = 50;
        }

        public static class DriveType
        {
            public const int DriveTypeNameMinLength = 2;
            public const int DriveTypeNameMaxLength = 30;
        }

        public static class FuelType
        {
            public const int FuelTypeNameMinLength = 2;
            public const int FuelTypeNameMaxLength = 30;
        }

        public static class GearboxType
        {
            public const int GearboxTypeNameMinLength = 2;
            public const int GearboxTypeNameMaxLength = 50;
        }

        public static class Vehicle
        {
            public const int VehicleVinNumberMinLength = 8;
            public const int VehicleVinNumberMaxLength = 17;

            public const int VehicleRegistrationNumberMinLength = 3;
            public const int VehicleRegistrationNumberMaxLength = 15;

            public const int VehicleYearMinValue = 1900;
            public const int VehicleYearMaxValue = 2100;
        }

        public static class Customer
        {
            public const int CustomerNameMinLength = 2;
            public const int CustomerNameMaxLength = 150;

            public const int CustomerPhoneMaxLength = 80;

            public const int CustomerCompanyNameMaxLength = 80;
        }

        public static class Garage
        {
            public const int GarageNameMinLength = 2;
            public const int GarageNameMaxLength = 50;

            public const int GaragePhoneMaxLength = 30;

            public const int GarageWorkingHoursMaxLength = 100;

            public const int GarageCoordinatesMaxLength = 40;
        }

        public static class Setting
        {
            public const int SettingNameMinLength = 2;
            public const int SettingNameMaxLength = 80;

            public const int SettingValueMinLength = 1;
            public const int SettingValueMaxLength = 300;
        }
    }
}
