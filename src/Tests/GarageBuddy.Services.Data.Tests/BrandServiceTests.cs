namespace GarageBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using Common;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Tests.Common;

    using Models.Vehicle.Brand;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class BrandServiceTests
    {
        private readonly ICollection<Brand> brands = new List<Brand>()
        {
            new() { Id = new Guid("00000000-0000-0000-0000-000000000001"), BrandName = "Audi" },
            new() { Id = new Guid("00000000-0000-0000-0000-000000000002"), BrandName = "BMW" },
            new() { Id = new Guid("00000000-0000-0000-0000-000000000003"), BrandName = "Mercedes" },
            new() { Id = new Guid("00000000-0000-0000-0000-000000000004"), BrandName = "Volkswagen" },
            new() { Id = new Guid("00000000-0000-0000-0000-000000000005"), BrandName = "Volvo" },
            new() { Id = new Guid("00000000-0000-0000-0000-000000000006"), BrandName = "Yugo", IsDeleted = true, },
        }.AsReadOnly();

        private IMapper mapper = null!;
        private IDeletableEntityRepository<Brand, Guid> repository = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<Brand, Guid>.Instance;

            foreach (var item in this.brands)
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
        public async Task GetAllAsync_ShouldReturnAllBrands()
        {
            TearDown();
            await SetUp();
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(this.brands.Count - 1));
        }

        [Test]
        [Order(2)]
        public async Task GetAllAsyncWithDeleted_ShouldReturnAllBrands()
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.GetAllAsync(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted);

            Assert.That(result.Count, Is.EqualTo(this.brands.Count));
        }

        [Test]
        [Order(3)]
        public async Task GetAllAsync_WithQueryOptions_ShouldReturnAllBrands()
        {
            var service = new BrandService(this.repository, this.mapper);

            var queryOptions = new QueryOptions<BrandServiceModel>
            {
                Take = 10,
                Skip = 0,
            };

            var result = await service.GetAllAsync(queryOptions);

            Assert.That(result.TotalCount, Is.EqualTo(this.brands.Count - 1));
        }

        [Test]
        [Order(4)]
        public async Task GetAllSelectAsync_ShouldReturnAllBrands()
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.GetAllSelectAsync();

            Assert.That(result.Count, Is.EqualTo(this.brands.Count));
        }

        [Test]
        [TestCase("00000000-0000-0000-0000-000000000002")]
        [Order(5)]
        public async Task GetAsync_ShouldReturnBrand(string id)
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.GetAsync(Guid.Parse(id));

            Assert.That(result.Data.Id.ToString(), Is.EqualTo(id));
        }

        [Test]
        [Order(6)]
        public async Task GetModelCountByBrandAsync_ShouldReturnModelCount()
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.GetModelCountByBrandAsync(20, false);

            Assert.That(result.Count, Is.EqualTo(this.brands.Count - 1));
        }

        [Test]
        [Order(7)]
        [TestCase(2)]
        public async Task GetModelCountByBrandAsync_ShouldReturnLessThanModelCount(int limit)
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.GetModelCountByBrandAsync(limit, false);

            Assert.That(result.Count, Is.EqualTo(limit));
        }

        [Test]
        [Order(8)]
        [TestCase("Audi")]
        public async Task BrandNameExistsAsync_ShouldReturnTrue(string brandName)
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.BrandNameExistsAsync(brandName, Guid.Empty);

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(9)]
        [TestCase("Audi", "00000000-0000-0000-0000-000000000001")]
        public async Task BrandNameExistsAsync_ShouldReturnFalse(string brandName, string id)
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.BrandNameExistsAsync(brandName, Guid.Parse(id));

            Assert.That(result, Is.False);
        }

        [Test]
        [Order(10)]
        [TestCase("Yugo")]
        public async Task BrandNameExistsAsync_ShouldReturnTrueForDeletedBrand(string brandName)
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.BrandNameExistsAsync(brandName, Guid.Empty);

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(11)]
        [TestCase("dwqdqw")]
        public async Task BrandNameExistAsync_ShouldReturnFalseForNonExistingBrand(string brandName)
        {
            var service = new BrandService(this.repository, this.mapper);

            var result = await service.BrandNameExistsAsync(brandName, Guid.Empty);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CreateAsync_ShouldCreateBrand()
        {
            var service = new BrandService(this.repository, this.mapper);

            var brand = new BrandServiceModel
            {
                BrandName = "Test",
            };

            var result = await service.CreateAsync(brand);

            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        [TestCase("Audi")]
        [Order(100)]
        public async Task CreateAsync_ShouldNotCreateBrand(string brandName)
        {
            var service = new BrandService(this.repository, this.mapper);

            var brand = new BrandServiceModel
            {
                BrandName = brandName,
            };

            var result = await service.CreateAsync(brand);

            Assert.That(result.Succeeded, Is.False);
        }

        [Test]
        [TestCase("Audi", "00000000-0000-0000-0000-000000000001")]
        public async Task EditAsync_ShouldEditBrand(string brandName, string id)
        {
            var service = new BrandService(this.repository, this.mapper);

            var brand = new BrandServiceModel
            {
                BrandName = brandName,
            };

            var result = await service.EditAsync(Guid.Parse(id), brand);

            Assert.That(result.Succeeded, Is.True);
        }

        [Test]
        [TestCase("Audi", "00000000-0000-0000-0000-000000000001")]
        public async Task EditAsync_ShouldHaveChangedBrandName(string brandName, string id)
        {
            var service = new BrandService(this.repository, this.mapper);

            var brand = new BrandServiceModel
            {
                BrandName = brandName,
            };

            await service.EditAsync(Guid.Parse(id), brand);

            var editedBrand = await service.GetAsync(Guid.Parse(id));

            Assert.That(editedBrand.Data.BrandName, Is.EqualTo(brandName));
        }
    }
}
