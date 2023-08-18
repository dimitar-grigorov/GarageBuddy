namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Customer;

    public interface ICustomerService
    {
        public Task<ICollection<CustomerSelectServiceModel>> GetAllSelectAsync();

        public Task<ICollection<CustomerListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.Deleted);

        public Task<bool> ExistsAsync(Guid id);

        public Task<IResult<CustomerServiceModel>> GetAsync(Guid id);

        public Task<IResult<Guid>> CreateAsync(CustomerServiceModel model);

        public Task<IResult> EditAsync(Guid id, CustomerServiceModel model);
    }
}
