namespace GarageBuddy.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using GarageBuddy.Data;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Models;
    using GarageBuddy.Data.Repositories;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    using Moq;

    using Xunit;

    public class SettingsServiceTests
    {
        [Fact]
        public void GetCountShouldReturnCorrectNumber()
        {
            var repository = new Mock<IDeletableEntityRepository<Setting, Guid>>();
            repository.Setup(r => r.All()).Returns(new List<Setting>
                                                        {
                                                            new Setting(),
                                                            new Setting(),
                                                            new Setting(),
                                                        }.AsQueryable());
            var service = new SettingsService(repository.Object);
            Assert.Equal(3, service.GetCount());
            repository.Verify(x => x.All(), Times.Once);
        }

        [Fact]
        public async Task GetCountShouldReturnCorrectNumberUsingDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: "SettingsTestDb").Options;
            await using var dbContext = new ApplicationDbContext(options);
            dbContext.Settings.Add(new Setting()
            {
                Name = "Test",
                Value = "Test",
            });
            dbContext.Settings.Add(new Setting()
            {
                Name = "Test",
                Value = "Test",
            });
            dbContext.Settings.Add(new Setting()
            {
                Name = "Test",
                Value = "Test",
            });
            await dbContext.SaveChangesAsync();

            using var repository = new EfDeletableEntityRepository<Setting, Guid>(dbContext);
            var service = new SettingsService(repository);
            Assert.Equal(3, service.GetCount());
        }
    }
}
