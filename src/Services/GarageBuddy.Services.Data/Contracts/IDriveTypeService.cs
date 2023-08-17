namespace GarageBuddy.Services.Data.Contracts
{
    using GarageBuddy.Services.Data.Models.Vehicle.Brand;
    using GarageBuddy.Services.Data.Models.Vehicle.BrandModel;

    using Models.Vehicle.DriveType;

    public interface IDriveTypeService
    {
        public Task<ICollection<DriveTypeServiceModel>> GetAllAsync();

        public Task<ICollection<DriveTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<IResult<int>> CreateAsync(DriveTypeServiceModel model);
    }
}
