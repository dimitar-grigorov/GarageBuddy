namespace GarageBuddy.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models.Vehicle;
    using GarageBuddy.Tests.Common;

    using Models.Vehicle.FuelType;

    using NUnit.Framework;

    using Services;

    [TestFixture]
    public class FuelTypeServiceTests
    {
        private readonly ICollection<FuelType> fuelTypes = new List<FuelType>
        {
            new() { Id = 1, FuelName = "Petrol", IsDeleted = true },
            new() { Id = 2, FuelName = "Diesel", IsDeleted = false },
            new() { Id = 3, FuelName = "Electric", IsDeleted = false },
        }.AsReadOnly();

        private IMapper mapper = null!;
        private IDeletableEntityRepository<FuelType, int> repository = null!;

        [SetUp]
        public async Task SetUp()
        {
            this.mapper = MapperMock.Instance;
            this.repository = DeletableEntityRepositoryMock<FuelType, int>.Instance;

            foreach (var item in this.fuelTypes)
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
        public async Task GetAllAsync_ShouldReturnAllFuelTypes()
        {
            var service = new FuelTypeService(this.repository, this.mapper);

            var result = await service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(this.fuelTypes.Count));
        }

        [Test]
        [Order(2)]
        public async Task GetAllSelectAsync_ShouldReturnAllFuelTypes()
        {
            var service = new FuelTypeService(this.repository, this.mapper);

            var result = await service.GetAllSelectAsync();

            Assert.That(result.Count, Is.EqualTo(this.fuelTypes.Count));
            Assert.That(result, Is.All.Not.Null);
            Assert.That(result.ElementAt(0).FuelName, Is.EqualTo("Diesel"));
        }

        [Test]
        [Order(3)]
        public async Task ExistsAsync_ShouldReturnTrueIfFuelTypeExists()
        {
            var service = new FuelTypeService(this.repository, this.mapper);

            var result = await service.ExistsAsync(1);

            Assert.That(result, Is.True);
        }

        [Test]
        [Order(4)]
        public async Task ExistsAsync_ShouldReturnFalseIfFuelTypeDoesNotExist()
        {
            var service = new FuelTypeService(this.repository, this.mapper);

            var result = await service.ExistsAsync(4);

            Assert.That(result, Is.False);
        }

        [Test]
        [Order(6)]
        public async Task EditAsync_ShouldEditFuelType()
        {
            var service = new FuelTypeService(this.repository, this.mapper);
            var result = await service.EditAsync(1, new FuelTypeServiceModel { FuelName = "Hybrid" });
            var model = await service.GetAsync(1);

            Assert.That(model.Data.FuelName, Is.EqualTo("Hybrid"));
            Assert.That(result.Succeeded, Is.True);
        }
    }
}
