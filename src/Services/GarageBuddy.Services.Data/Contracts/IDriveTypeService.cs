namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.DriveType;

    public interface IDriveTypeService
    {
        public Task<ICollection<DriveTypeSelectServiceModel>> GetAllSelectAsync();
    }
}
