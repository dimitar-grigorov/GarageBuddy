namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.DriveType;

    public interface IDriveTypeService
    {
        public Task<ICollection<DriveTypeServiceModel>> GetAllAsync();

        public Task<ICollection<DriveTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<bool> ExistsAsync(int id);

        public Task<IResult<DriveTypeServiceModel>> GetAsync(int id);

        public Task<IResult<int>> CreateAsync(DriveTypeServiceModel model);

        public Task<IResult> EditAsync(int id, DriveTypeServiceModel model);
    }
}
