namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;

    using Contracts;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Models.Customer;

    public class CustomerService : BaseService<Customer, Guid>, ICustomerService
    {
        private readonly IDeletableEntityRepository<Customer, Guid> customerRepository;

        public CustomerService(
            IDeletableEntityRepository<Customer, Guid> entityRepository,
            IMapper mapper)
            : base(entityRepository, mapper)
        {
            this.customerRepository = entityRepository;
        }

        public async Task<ICollection<CustomerSelectServiceModel>> GetAllSelectAsync()
        {
            return await customerRepository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted)
                .OrderBy(c => c.IsDeleted)
                .ThenBy(c => c.Name)
                .ThenBy(c => c.Phone)
                .Select(c => new CustomerSelectServiceModel
                {
                    Id = c.Id.ToString(),
                    CustomerName = c.Name,
                    Phone = c.Phone ?? NoValue,
                }).ToListAsync();
        }
    }
}
