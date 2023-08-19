namespace GarageBuddy.Tests.Common
{
    using Data;

    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// This is a mock of <see cref="ApplicationDbContext"/>.
    /// </summary>
    public static class DbContextMock
    {
        /// <summary>
        /// Gets or sets the instance of <see cref="ApplicationDbContext"/>.
        /// </summary>
        public static ApplicationDbContext DbContext { get; set; } = null!;

        /// <summary>
        /// The method initializes the ApplicationDbContext.
        /// </summary>
        /// <returns>Returns a <see cref="Task{TResult}"/> with <see cref="ApplicationDbContext"/>.</returns>
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

        /// <summary>
        /// The DisposeAsync() method in DbContextMock is responsible
        /// for disposing the ApplicationDbContext instance and then
        /// deleting the in-memory database.
        /// </summary>
        /// <returns>Returns a <see cref="Task"/>.</returns>
        public static async Task DisposeAsync()
        {
            await DbContext.Database.EnsureDeletedAsync();
            await DbContext.DisposeAsync();
        }
    }
}
