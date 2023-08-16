namespace GarageBuddy.Services.Data.Contracts
{
    using Models.Customer;

    public interface ICustomerService
    {
        public Task<ICollection<CustomerSelectServiceModel>> GetAllSelectAsync();
    }
}
