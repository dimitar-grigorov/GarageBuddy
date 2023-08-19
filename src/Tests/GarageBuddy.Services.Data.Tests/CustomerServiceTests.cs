namespace GarageBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;
    using GarageBuddy.Tests.Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Customer;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class CustomerServiceTests
    {
        private readonly ICollection<Customer> customers = new List<Customer>()
        {
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Customer 1",
                Address = "Address 1",
                Phone = "1234567890",
                CompanyName = "Company 1",
                ImageUrl = "https://avatars.githubusercontent.com/u/5540328?v=4",
                Description = "Description 1",
                ApplicationUserId = null,
                IsDeleted = false,
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Customer 2",
                Address = "Address 2",
                Phone = "1234567891",
                CompanyName = "Company 2",
                ImageUrl = "https://avatars.githubusercontent.com/u/5540328?v=4",
                Description = "Description 2",
                ApplicationUserId = null,
                IsDeleted = false,
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Customer 3",
                Address = "Address 3",
                Phone = "1234567892",
                CompanyName = "Company 3",
                ImageUrl = "https://avatars.githubusercontent.com/u/5540328?v=4",
                Description = "Description 3",
                ApplicationUserId = null,
                IsDeleted = true,
            },
        }.AsReadOnly();

        private IMapper mapper;
        private IDeletableEntityRepository<Customer, Guid> repository = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<Customer, Guid>.Instance;

            foreach (var item in this.customers)
            {
                await this.repository.AddAsync(item);
            }

            await this.repository.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            this.repository.Dispose();
        }

        [Test]
        [Order(1)]
        public async Task GetAllSelectAsync_ShouldReturnAllCustomers()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.GetAllSelectAsync();

            Assert.That(result.Count, Is.EqualTo(this.customers.Count));
        }

        [Test]
        [Order(2)]
        public async Task GetAllAsync_ShouldReturnAllCustomers()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(this.customers.Count));
        }

        [Test]
        [Order(3)]
        public async Task GetAllAsync_WithReadOnly_ShouldReturnAllCustomers()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.GetAllAsync(ReadOnlyOption.ReadOnly);

            Assert.That(result.Count, Is.EqualTo(this.customers.Count));
        }

        [Test]
        [Order(4)]
        public async Task GetAllAsync_WithDeleted_ShouldReturnAllCustomers()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.GetAllAsync(ReadOnlyOption.Normal);

            Assert.That(result.Count, Is.EqualTo(this.customers.Count));
        }

        [Test]
        [Order(5)]
        public async Task GetAllAsync_WithReadOnlyAndDeleted_ShouldReturnAllCustomers()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.GetAllAsync(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted);

            Assert.That(result.Count, Is.EqualTo(this.customers.Count - 1));
        }

        [Test]
        [Order(6)]
        public async Task ExistAsync_WithExistingId_ShouldReturnTrue()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.ExistsAsync(Guid.Parse("00000000-0000-0000-0000-000000000001"));

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(7)]
        public async Task ExistAsync_WithNotExistingId_ShouldReturnFalse()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);

            var result = await service.ExistsAsync(Guid.Parse("00000000-0000-0000-0000-000000000004"));

            Assert.That(result, Is.False);
        }

        [Test]
        [Order(8)]
        public async Task CreateAsync_ShouldCreateCustomer()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);
            var model = new CustomerServiceModel()
            {
                Name = "Customer 4",
                Address = "Address 4",
                Phone = "1234567893",
                CompanyName = "Company 4",
                ImageUrl = "https://avatars.githubusercontent.com/u/5540328?v=4",
                Description = "Description 4",
            };

            var result = await service.CreateAsync(model);

            var entity = await this.repository.All(ReadOnlyOption.ReadOnly)
                .FirstOrDefaultAsync(x => x.Id == result.Data);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(entity.Name, Is.EqualTo("Customer 4"));
                Assert.That(entity.Address, Is.EqualTo("Address 4"));
                Assert.That(entity.Phone, Is.EqualTo("1234567893"));
                Assert.That(entity.CompanyName, Is.EqualTo("Company 4"));
                Assert.That(entity.ImageUrl, Is.EqualTo("https://avatars.githubusercontent.com/u/5540328?v=4"));
                Assert.That(entity.Description, Is.EqualTo("Description 4"));
            });
        }

        [Test]
        [Order(9)]
        public async Task EditAsync_ShouldEditCustomer()
        {
            var service = new CustomerService(this.repository, this.mapper, null!);
            var model = new CustomerServiceModel()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Customer 1 Edited",
                Address = "Address 1 Edited",
                Phone = "1234567890",
                CompanyName = "Company 1 Edited",
                ImageUrl = "https://avatars.githubusercontent.com/u/5540328?v=4",
                Description = "Description 1 Edited",
            };

            var result = await service.EditAsync(model.Id, model);

            var entity = await this.repository.All(ReadOnlyOption.ReadOnly)
                .FirstOrDefaultAsync(x => x.Id == model.Id);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(entity.Name, Is.EqualTo("Customer 1 Edited"));
                Assert.That(entity.Address, Is.EqualTo("Address 1 Edited"));
                Assert.That(entity.Phone, Is.EqualTo("1234567890"));
                Assert.That(entity.CompanyName, Is.EqualTo("Company 1 Edited"));
                Assert.That(entity.ImageUrl, Is.EqualTo("https://avatars.githubusercontent.com/u/5540328?v=4"));
                Assert.That(entity.Description, Is.EqualTo("Description 1 Edited"));
            });
        }
    }
}
