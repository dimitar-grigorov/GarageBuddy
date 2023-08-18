namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Vehicle.GearboxType;

    public interface IGearboxTypeService
    {
        public Task<ICollection<GearboxTypeServiceModel>> GetAllAsync();

        public Task<ICollection<GearboxTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<bool> ExistsAsync(int id);

        public Task<IResult<GearboxTypeServiceModel>> GetAsync(int id);

        public Task<IResult<int>> CreateAsync(GearboxTypeServiceModel model);

        public Task<IResult> EditAsync(int id, GearboxTypeServiceModel model);
    }
}
