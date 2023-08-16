namespace GarageBuddy.Services.Data.Contracts
{
    using GarageBuddy.Services.Data.Models.Vehicle.Brand;

    using Models.Vehicle.DriveType;

    public interface IDriveTypeService
    {
        public Task<ICollection<DriveTypeServiceModel>> GetAllAsync();

        public Task<ICollection<DriveTypeSelectServiceModel>> GetAllSelectAsync();
    }
}
