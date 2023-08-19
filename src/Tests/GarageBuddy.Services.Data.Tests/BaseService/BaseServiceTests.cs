namespace GarageBuddy.Services.Data.Tests.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using BaseService;

    using Common;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Tests.Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle.Brand;

    using NUnit.Framework;

    using static GarageBuddy.Common.Constants.EntityValidationConstants.Brand;

    [TestFixture]
    public class BaseServiceTests
    {
        private readonly ICollection<Brand> brandNotes = new List<Brand>()
        {
            new()
            {
                Id = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                BrandName = "Brand 1",
                IsDeleted = false,
            },
            new()
            {
                Id = Guid.Parse("9983bb6a-1c02-4d43-b9ba-cf3723d0932a"),
                BrandName = "Brand 2",
                IsDeleted = true,
            },
            new()
            {
                Id = Guid.Parse("e94ed14e-ad6e-4a31-a362-047c7eca1b19"),
                BrandName = "Brand 3",
                IsDeleted = false,
            },
        }.AsReadOnly();

        private IMapper mapper;
        private IDeletableEntityRepository<Brand, Guid> repository;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<Brand, Guid>.Instance;

            foreach (var item in this.brandNotes)
            {
                await this.repository.AddAsync(item);
            }
        }

        [TearDown]
        public void TearDown()
        {
            this.repository.Dispose();
        }

        [Test]
        public async Task GetAllAsyncShouldReturnAllNotDeletedEntities()
        {
            // Arrange
            BaseService<Brand, Guid> baseService = new(this.repository, this.mapper);

            // Act
            var models = await baseService.GetAllAsync<BrandServiceModel>();

            // Assert
            Assert.That(models, Has.Count.EqualTo(2), "Entities count is not correct.");
            Assert.That(models, Has.All.Matches<object>(x => x is BrandServiceModel), "Some entities are not of the correct type.");
        }

        [Test]
        public async Task GetAllAsyncShouldReturnAllEntitiesWithDeleted()
        {
            // Arrange
            BaseService<Brand, Guid> baseService = new(this.repository, this.mapper);

            // Act
            var models = await baseService.GetAllAsync<BrandServiceModel>(new()
            {
                IncludeDeleted = DeletedFilter.Deleted,
            });

            // Assert
            Assert.That(models.Data, Has.Count.EqualTo(3), "Entities count is not correct.");
            Assert.That(models.Data, Has.All.Matches<object>(x => x is BrandServiceModel), "Some entities are not of the correct type.");
        }

        [Test]
        public async Task GetAllAsyncShouldReturnAllEntitiesInCorrectOrder()
        {
            // Arrange
            BaseService<Brand, Guid> baseService = new(this.repository, this.mapper);

            // Act
            var models = await baseService.GetAllAsync<BrandServiceModel>(new()
            {
                OrderOptions = new List<OrderOption<BrandServiceModel>>
                {
                    new(e => e.BrandName, OrderByOrder.Ascending),
                },
                IncludeDeleted = DeletedFilter.Deleted,
            });

            // Assert
            var brandList = await this.repository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted).ToListAsync();
            Assert.That(models.Data, Has.Count.EqualTo(3), "Entities count is not correct.");
            Assert.That(models.Data, Has.All.Matches<object>(x => x is BrandServiceModel), "Some entities are not of the correct type.");
            Assert.That(models.Data.First().BrandName, Is.EqualTo(brandList.First().BrandName), "Entities are not in correct order.");
        }

        [Test]
        [TestCase("8d162afd-bcb2-4ccb-81c0-96965f7177d0")]
        public void GetAsyncShouldThrowWhenEntityDoesNotExist(string id)
        {
            // Arrange
            BaseService<Brand, Guid> baseService = new(this.repository, this.mapper);
            var guid = Guid.Parse(id);

            // Act
            async Task Code() => await baseService.GetAsync<BrandServiceModel>(guid);

            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await Code(), "Should throw when entity is not found.");
        }

        [Test]
        [TestCase("9983bb6a-1c02-4d43-b9ba-cf3723d0932a")]
        public async Task GetAsyncShouldReturnEntityModelWithGivenId(string id)
        {
            // Arrange
            BaseService<Brand, Guid> baseService = new(this.repository, this.mapper);
            Guid guid = Guid.Parse(id);

            // Act
            var model = await baseService.GetAsync<BrandServiceModel>(guid);

            // Assert
            var brandList = await this.repository.All(ReadOnlyOption.ReadOnly, DeletedFilter.Deleted).ToListAsync();
            var brand = brandList.First(rn => rn.Id == guid);
            Assert.Multiple(() =>
            {
                Assert.That(brand, Is.Not.Null, "Entity does not exist.");
                Assert.That(model != null, Is.True, "Entity is not of correct type.");
                if (model != null)
                {
                    Assert.That(model.BrandName, Is.EqualTo(brand.BrandName), "Entities do not match.");
                }
            });
        }

        [Test]
        public void ValidateShouldReturnFalse()
        {
            // Arrange
            BaseServiceTest<Brand, Guid> baseService = new(this.repository, this.mapper);

            var shortBrand = new Brand()
            {
                BrandName = new string('*', BrandNameMaxLength + 4),
            };

            var longBrand = new Brand()
            {
                BrandName = new string('*', BrandNameMaxLength + 1),
            };

            // Act
            var shortResult = baseService.ValidateModel(shortBrand);
            var longResult = baseService.ValidateModel(longBrand);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shortResult, Is.False, "Validation result should be false.");
                Assert.That(longResult, Is.False, "Validation result should be false.");
            });
        }

        [Test]
        public void ValidateShouldReturnTrue()
        {
            // Arrange
            BaseServiceTest<Brand, Guid> baseService = new(this.repository, this.mapper);

            var shortModel = new Brand()
            {
                BrandName = new string('*', BrandNameMaxLength),
            };

            var longModel = new Brand()
            {
                BrandName = new string('*', BrandNameMaxLength),
            };

            // Act
            bool shortResult = baseService.ValidateModel(shortModel);
            bool longResult = baseService.ValidateModel(longModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(shortResult, Is.True, "Validation result should be true.");
                Assert.That(longResult, Is.True, "Validation result should be true.");
            });
        }

        [Test]
        public void ValidateShouldThrowWhenModelIsNull()
        {
            // Arrange
            BaseServiceTest<Brand, Guid> baseService = new(this.repository, this.mapper);
            BrandServiceModel serviceModel = null;

            // Act
            // ReSharper disable once ExpressionIsAlwaysNull
            void Code()
            {
                baseService.ValidateModel(serviceModel);
            }

            // Assert
            Assert.Throws<ArgumentNullException>(Code, "Method should throw when model is null.");
        }
    }
}
