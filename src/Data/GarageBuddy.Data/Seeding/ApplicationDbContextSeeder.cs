namespace GarageBuddy.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Common;

    using Data;

    using Job;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using Vehicle;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new JobStatusSeeder(),

                              // new JobItemTypeSeeder(),
                              new BrandSeeder(),
                              new BrandModelSeeder(), // Always after BrandSeeder
                              new DriveTypeSeeder(),
                              new FuelTypeSeeder(),
                              new GearboxTypeSeeder(),
                              new GarageSeeder(),
                              new CustomerSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
