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

    using Models;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class GarageServiceTests
    {
        private readonly ICollection<Garage> garages = new List<Garage>
        {
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Test Garage 1",
                Address = "Test Address 1",
                Email = "Test Email 1",
                Phone = "Test Phone 1",
                ImageUrl = "Test ImageUrl 1",
                WorkingHours = "Test WorkingHours 1",
                Description = "Test Description 1",
                Coordinates = "Test Coordinates 1",
                IsDeleted = true,
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Test Garage 2",
                Address = "Test Address 2",
                Email = "Test Email 2",
                Phone = "Test Phone 2",
                ImageUrl = "Test ImageUrl 2",
                WorkingHours = "Test WorkingHours 2",
                Description = "Test Description 2",
                Coordinates = "Test Coordinates 2",
                IsDeleted = true,
            },
            new()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Test Garage 3",
                Address = "Test Address 3",
                Email = "Test Email 3",
                Phone = "Test Phone 3",
                ImageUrl = "Test ImageUrl 3",
                WorkingHours = "Test WorkingHours 3",
                Description = "Test Description 3",
                Coordinates = "Test Coordinates 3",
                IsDeleted = false,
            },
        }.AsReadOnly();

        private IMapper mapper;
        private IDeletableEntityRepository<Garage, Guid> repository = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<Garage, Guid>.Instance;

            foreach (var item in this.garages)
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
        public async Task GetAllAsync_WithReadOnlyOptionNormal_ShouldReturnAllGarages()
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(this.garages.Count));
        }

        [Test]
        [Order(2)]
        public async Task GetAllAsync_WithReadOnlyOptionReadOnly_ShouldReturnAllGarages()
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.GetAllAsync(ReadOnlyOption.ReadOnly);

            Assert.That(result.Count, Is.EqualTo(this.garages.Count));
        }

        [Test]
        [Order(3)]
        public async Task GetAllAsync_WithReadOnlyOptionNormalAndIncludeDeletedDeleted_ShouldReturnAllGarages()
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.GetAllAsync(ReadOnlyOption.Normal, DeletedFilter.Deleted);

            Assert.That(result.Count, Is.EqualTo(this.garages.Count));
        }

        [Test]
        [Order(4)]
        [TestCase("00000000-0000-0000-0000-000000000001")]
        public async Task ExistsAsync_WithExistingId_ShouldReturnTrue(string id)
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.ExistsAsync(Guid.Parse(id));

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(5)]
        [TestCase("00000000-0000-0000-0000-000000000004")]
        public async Task ExistsAsync_WithNonExistingId_ShouldReturnFalse(string id)
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.ExistsAsync(Guid.Parse(id));

            Assert.That(result, Is.False);
        }

        [Test]
        [Order(6)]
        [TestCase("00000000-0000-0000-0000-000000000001", "Test Address 1")]
        public async Task GetAsync_WithExistingId_ShouldReturnGarage(string id, string address)
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.GetAsync(Guid.Parse(id));

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data.Address, Is.EqualTo(address));
        }

        [Test]
        [Order(8)]
        public async Task EditAsync_WithExistingId_ShouldEditGarage()
        {
            var service = new GarageService(this.repository, this.mapper);
            var garage = new GarageServiceModel()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Test Garage 1",
                Address = "Test Address 1",
                Email = "Test Email 1",
                Phone = "Test Phone 1",
                ImageUrl = "Test ImageUrl 1",
                WorkingHours = "Test WorkingHours 1",
                Description = "Test Description 1",
                Coordinates = "Test Coordinates 1",
                IsDeleted = true,
            };

            var result = await service.EditAsync(garage.Id, garage);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        [Order(9)]
        public async Task AtLeastOneActiveGarageExistsAsync_WithExistingActiveGarage_ShouldReturnTrue()
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.AtLeastOneActiveGarageExistsAsync(Guid.Empty);

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(10)]
        [TestCase("00000000-0000-0000-0000-000000000003")]
        public async Task AtLeastOneActiveGarageExistsAsync_WithExistingActiveGarage_ShouldReturnFalse(string id)
        {
            var service = new GarageService(this.repository, this.mapper);
            var result = await service.AtLeastOneActiveGarageExistsAsync(Guid.Parse(id));

            Assert.That(result, Is.False);
        }
    }
}
