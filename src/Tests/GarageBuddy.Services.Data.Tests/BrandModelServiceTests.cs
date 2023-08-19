namespace GarageBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Common;

    using Contracts;

    using GarageBuddy.Common.Core.Enums;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Tests.Common;

    using Models.Vehicle.BrandModel;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class BrandModelServiceTests
    {
        private readonly ICollection<BrandModel> brandModels = new List<BrandModel>()
        {
            new()
            {
                Id = Guid.Parse("6460eef6-1aee-442f-a39f-9e43319ab49d"),
                ModelName = "Model 1",
                IsDeleted = false,
                BrandId = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff02"),
                Brand = new Brand()
                {
                    Id = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff02"),
                    BrandName = "Brand 2",
                    IsDeleted = false,
                },
            },
            new()
            {
                Id = Guid.Parse("250c464f-f66b-4d37-ad1b-2e9c56d7aeab"),
                ModelName = "Model 2",
                IsDeleted = true,
                BrandId = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                Brand = new Brand()
                {
                    Id = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                    BrandName = "Brand 1",
                    IsDeleted = false,
                },
            },
            new()
            {
                Id = Guid.Parse("e94ed14e-ad6e-4a31-a362-047c7eca1b19"),
                ModelName = "Model 3",
                IsDeleted = false,
                BrandId = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                Brand = new Brand()
                {
                    Id = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                    BrandName = "Brand 1",
                    IsDeleted = false,
                },
            },
        }.AsReadOnly();

        private IMapper mapper;
        private IDeletableEntityRepository<BrandModel, Guid> repository = null!;
        private IBrandService brandService;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<BrandModel, Guid>.Instance;
            this.brandService = BrandServiceMock.Instance;

            foreach (var item in this.brandModels)
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
        public async Task GetAllAsync_WithNoParameters_ShouldReturnAllBrandModels()
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAllAsync(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(2));
            });

            Assert.IsTrue(true);
        }

        [Test]
        public async Task GetAllAsync_WithNoParameters_ShouldReturnAllBrandModelsOrderedByBrandName()
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAllAsync(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.ElementAt(0).BrandName, Is.EqualTo("Brand 2"));
                Assert.That(result.ElementAt(1).BrandName, Is.EqualTo("Brand 1"));
            });
        }

        [Test]
        [Order(6)]
        public async Task GetAllAsync_WithNoParameters_ShouldReturnAllBrandModelsOrderedByModelName()
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAllAsync(ReadOnlyOption.ReadOnly, DeletedFilter.NotDeleted);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.ElementAt(0).ModelName, Is.EqualTo("Model 1"));
                Assert.That(result.ElementAt(1).ModelName, Is.EqualTo("Model 3"));
            });
        }

        [Test]
        public async Task GetAllAsync_WithParameters_ShouldReturnAllBrandModelsFilteredByBrandName()
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var queryOptions = new QueryOptions<BrandModelListServiceModel>()
            {
                OrderOptions = new List<OrderOption<BrandModelListServiceModel>>
                {
                    new(e => e.BrandName, OrderByOrder.Descending),
                },
            };

            var result = await brandModelService
                .GetAllAsync(queryOptions);

            // Assert
            Assert.Multiple(() =>
                {
                    Assert.That(result.Data, Is.Not.Null);
                    Assert.That(result.Data.Count, Is.EqualTo(2));
                    Assert.That(result.Data.ElementAt(0).BrandName, Is.EqualTo("Brand 2"));
                });
        }

        [Test]
        [TestCase("2bfff802-5afb-4bbb-96b3-27c98161ff00")]
        [Order(3)]
        public async Task GetAllSelectAsync_ShouldReturnAllBrandModelsAsSelectListItems(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAllSelectAsync(Guid.Parse(id));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Count, Is.EqualTo(3));
                Assert.That(result.ElementAt(0).ModelName, Is.EqualTo("Select Model"));
                Assert.That(result.ElementAt(1).ModelName, Is.EqualTo("Model 3"));
            });
        }

        [Test]
        [TestCase("2bfff802-5afb-4bbb-96b3-27c98161ff00")]
        [Order(4)]
        public async Task GetAllByBrandIdAsync_ShouldReturnAllBrandModelsByBrandId(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAllByBrandIdAsync(Guid.Parse(id),
                new QueryOptions<BrandModelListServiceModel>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.Data.ElementAt(0).ModelName, Is.EqualTo("Model 3"));
            });
        }

        [Test]
        [TestCase("6460eef6-1aee-442f-a39f-9e43319ab49d")]
        public async Task ExistsAsync_WithExistingId_ShouldReturnTrue(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.ExistsAsync(Guid.Parse(id));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("6460eef6-1aee-442f-a39f-9e43319ab49e")]
        public async Task ExistsAsync_WithNotExistingId_ShouldReturnFalse(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.ExistsAsync(Guid.Parse(id));

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("6460eef6-1aee-442f-a39f-9e43319ab49d")]
        [Order(2)]
        public async Task GetAsync_WithExistingId_ShouldReturnBrandModel(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAsync(Guid.Parse(id));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data.ModelName, Is.EqualTo("Model 1"));
            });
        }

        [Test]
        [TestCase("6460eef6-1aee-442f-a39f-9e43319ab49e")]
        public async Task GetAsync_WithNotExistingId_ShouldReturnNull(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAsync(Guid.Parse(id));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data, Is.Null);
            });
        }

        [Test]
        [TestCase("6460eef6-1aee-442f-a39f-9e43319ab49d")]
        [Order(1)]
        public async Task GetAsync_WithExistingId_ShouldReturnBrandModelWithBrand(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);

            // Act
            var result = await brandModelService.GetAsync(Guid.Parse(id));

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Data.ModelName, Is.EqualTo("Model 1"));
            });
        }

        [Test]
        [TestCase("6460eef6-1aee-442f-a39f-9e43319ab49e")]
        public async Task CreateAsync_WithNotExistingBrandId_ShouldReturnFalse(string id)
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);
            var brandModelServiceModel = new BrandModelServiceModel()
            {
                BrandId = Guid.Parse(id),
                ModelName = "Model 4",
            };

            // Act
            var result = await brandModelService.CreateAsync(brandModelServiceModel);

            // Assert
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        public async Task EditAsync_WithExistingBrandModel_ShouldReturnTrue()
        {
            // Arrange
            var brandModelService = new BrandModelService(this.repository, this.mapper, this.brandService);
            var brandModelId = Guid.Parse("6460eef6-1aee-442f-a39f-9e43319ab49d");
            var brandModelServiceModel = new BrandModelServiceModel()
            {
                Id = brandModelId,
                BrandId = Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00"),
                ModelName = "Model 122",
            };

            // Act
            var result = await brandModelService.EditAsync(brandModelId, brandModelServiceModel);

            var editedBrandModel = await brandModelService.GetAsync(brandModelId);

            // Assert
            Assert.IsTrue(result.Succeeded);
            Assert.Multiple(() =>
            {
                Assert.That(editedBrandModel.Data.ModelName, Is.EqualTo("Model 122"));
                Assert.That(editedBrandModel.Data.BrandId, Is.EqualTo(Guid.Parse("2bfff802-5afb-4bbb-96b3-27c98161ff00")));
            });
        }
    }
}
