namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Job.JobItemType;

    public interface IJobItemTypeService
    {
        public Task<ICollection<JobItemTypeServiceModel>> GetAllAsync();

        public Task<ICollection<JobItemTypeSelectServiceModel>> GetAllSelectAsync();

        public Task<IResult<Guid>> CreateAsync(JobItemTypeServiceModel model);

        public Task<IResult> EditAsync(Guid id, JobItemTypeServiceModel model);
    }
}
