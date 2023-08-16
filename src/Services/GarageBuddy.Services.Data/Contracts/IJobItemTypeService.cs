namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Job.JobItemType;

    public interface IJobItemTypeService
    {
        public Task<ICollection<JobItemTypeServiceModel>> GetAllAsync();

        public Task<ICollection<JobItemTypeSelectServiceModel>> GetAllSelectAsync();
    }
}
