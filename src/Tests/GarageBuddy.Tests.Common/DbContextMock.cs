namespace GarageBuddy.Tests.Common
{
    using Data;

    using Microsoft.EntityFrameworkCore;

    public static class DbContextMock
    {
        public static ApplicationDbContext DbContext { get; set; } = null!;

        public static async Task<ApplicationDbContext> InitializeDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: new Guid().ToString()).Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.Database.EnsureCreatedAsync();

            DbContext = dbContext;
            return dbContext;
        }

        public static async Task DisposeAsync()
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.DisposeAsync();
        }
    }
}
