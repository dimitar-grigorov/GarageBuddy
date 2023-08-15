namespace GarageBuddy.Services.Data.Services
{
    using AutoMapper;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;
    using GarageBuddy.Services.Data.Contracts;

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
    }
}
