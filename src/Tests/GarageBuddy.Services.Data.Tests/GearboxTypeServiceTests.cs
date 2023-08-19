namespace GarageBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Tests.Common;

    using Models.Vehicle.GearboxType;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class GearboxTypeServiceTests
    {
        private readonly ICollection<GearboxType> gearboxTypes = new List<GearboxType>()
        {
            new GearboxType()
            {
                Id = 1,
                GearboxTypeName = "Manual",
                IsDeleted = false,
            },
            new GearboxType()
            {
                Id = 2,
                GearboxTypeName = "Automatic",
                IsDeleted = false,
            },
            new GearboxType()
            {
                Id = 3,
                GearboxTypeName = "Semi-automatic",
                IsDeleted = true,
            },
        }.AsReadOnly();

        private IMapper mapper;
        private IDeletableEntityRepository<GearboxType, int> repository = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<GearboxType, int>.Instance;

            foreach (var item in this.gearboxTypes)
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
        public async Task GetAllAsync_ShouldReturnAllGearboxTypes()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);

            var result = await gearboxTypeService.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(this.gearboxTypes.Count));
        }

        [Test]
        [Order(2)]
        public async Task GetAllSelectAsync_ShouldReturnAllGearboxTypes()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);

            var result = await gearboxTypeService.GetAllSelectAsync();

            Assert.That(result.Count, Is.EqualTo(this.gearboxTypes.Count));
        }

        [Test]
        [Order(3)]
        public async Task ExistsAsync_ShouldReturnTrueIfGearboxTypeExists()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);

            var result = await gearboxTypeService.ExistsAsync(1);

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(4)]
        public async Task ExistsAsync_ShouldReturnFalseIfGearboxTypeDoesNotExist()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);

            var result = await gearboxTypeService.ExistsAsync(4);

            Assert.That(result, Is.False);
        }

        [Test]
        [Order(5)]
        public async Task GetAsync_ShouldReturnGearboxTypeIfGearboxTypeExists()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);

            var result = await gearboxTypeService.GetAsync(1);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Messages, Is.Empty);
            Assert.That(result.Data.Id, Is.EqualTo(1));
        }

        [Test]
        [Order(6)]
        public async Task GetAsync_ShouldReturnErrorIfGearboxTypeDoesNotExist()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);

            var result = await gearboxTypeService.GetAsync(4);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Messages, Is.Not.Empty);
            Assert.That(result.Data, Is.Null);
        }

        [Test]
        [Order(7)]
        public async Task EditAsync_ShouldEditGearboxTypeIfGearboxTypeExists()
        {
            var gearboxTypeService = new GearboxTypeService(this.repository, this.mapper);
            var model = new GearboxTypeServiceModel()
            {
                Id = 1,
                GearboxTypeName = "Manual",
                IsDeleted = false,
            };

            var result = await gearboxTypeService.EditAsync(model.Id, model);
            var resultModel = await gearboxTypeService.GetAsync(model.Id);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Messages, Is.Empty);
            Assert.That(resultModel.Data.Id, Is.EqualTo(1));
            Assert.That(resultModel.Data.GearboxTypeName, Is.EqualTo("Manual"));
        }
    }
}
