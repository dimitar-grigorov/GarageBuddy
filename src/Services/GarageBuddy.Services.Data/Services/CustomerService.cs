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

        private readonly IUserService userService;

        public CustomerService(
            IDeletableEntityRepository<Customer, Guid> entityRepository,
            IMapper mapper, IUserService userService)
            : base(entityRepository, mapper)
        {
            this.customerRepository = entityRepository;
            this.userService = userService;
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

        public async Task<ICollection<CustomerListServiceModel>> GetAllAsync(
            ReadOnlyOption asReadOnly = ReadOnlyOption.Normal,
            DeletedFilter includeDeleted = DeletedFilter.Deleted)
        {
            var result = await customerRepository.All(asReadOnly, includeDeleted)
                .Include(c => c.ApplicationUser)
                .OrderBy(c => c.IsDeleted)
                .ThenBy(c => c.Name)
                .ThenBy(c => c.Phone)
                .Select(c => new
                {
                    Customer = c,
                    c.ApplicationUser,
                })
                .Select(c => new CustomerListServiceModel
                {
                    Id = c.Customer.Id,
                    Name = c.Customer.Name,
                    Address = c.Customer.Address ?? string.Empty,
                    Phone = c.Customer.Phone ?? string.Empty,
                    Email = c.Customer.Email ?? string.Empty,
                    CompanyName = c.Customer.CompanyName ?? string.Empty,
                    ImageUrl = c.Customer.ImageUrl ?? string.Empty,
                    Description = c.Customer.Description ?? string.Empty,
                    ApplicationUserId = c.Customer.ApplicationUserId,
                    UserName = c.ApplicationUser != null ? c.ApplicationUser.UserName ?? string.Empty : string.Empty,
                }).ToListAsync();
            return result;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await customerRepository.ExistsAsync(id);
        }

        public async Task<IResult<CustomerServiceModel>> GetAsync(Guid id)
        {
            if (!await ExistsAsync(id))
            {
                return await Result<CustomerServiceModel>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Customer)));
            }

            var model = await base.GetAsync<CustomerServiceModel>(id);
            return await Result<CustomerServiceModel>.SuccessAsync(model);
        }

        public async Task<IResult<Guid>> CreateAsync(CustomerServiceModel model)
        {
            var isValid = ValidateModel(model);
            if (!isValid)
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotFound, nameof(Customer)));
            }

            // If user id is pointing to non existent user
            if (model.ApplicationUserId != null)
            {
                if (!await userService.ExistsAsync((Guid)model.ApplicationUserId))
                {
                    return await Result<Guid>.FailAsync(
                        string.Format(Errors.NoEntityWithPropertyValuesFound, "Customer", "User", model.ApplicationUserId));
                }
            }

            var serviceModel = this.Mapper.Map<Customer>(model);

            var entity = await customerRepository.AddAsync(serviceModel);
            await customerRepository.SaveChangesAsync();
            var id = entity?.Entity.Id ?? Guid.Empty;

            if (entity?.Entity.Id != Guid.Empty)
            {
                return await Result<Guid>.SuccessAsync(id);
            }

            return await Result<Guid>.FailAsync(string.Format(Errors.EntityNotCreated, nameof(Customer)));
        }

        public async Task<IResult> EditAsync(Guid id, CustomerServiceModel model)
        {
            if (!await ExistsAsync(id))
            {
                return await Result.FailAsync(string.Format(Errors.EntityNotFound, nameof(Customer)));
            }

            if (!ValidateModel(model))
            {
                return await Result<Guid>.FailAsync(string.Format(Errors.EntityModelStateIsNotValid, nameof(Customer)));
            }

            // If user id is pointing to non existent user
            if (model.ApplicationUserId != null)
            {
                if (!await userService.ExistsAsync((Guid)model.ApplicationUserId))
                {
                    return await Result<Guid>.FailAsync(
                        string.Format(Errors.NoEntityWithPropertyValuesFound, "Customer", "User", model.ApplicationUserId));
                }
            }

            await base.EditAsync(id, model);

            return await Result<Guid>.SuccessAsync();
        }
    }
}
