namespace GarageBuddy.Services.Data.Contracts
{
    using GarageBuddy.Services.Data.Models.Vehicle.BrandModel;

    using Models.Vehicle.GearboxType;

    public interface IGearboxTypeService
    {
        public Task<ICollection<GearboxTypeServiceModel>> GetAllAsync();

        public Task<ICollection<GearboxTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<IResult<int>> CreateAsync(GearboxTypeServiceModel model);
    }
}
