namespace GarageBuddy.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.EntityFrameworkCore;

    using Models;

    internal class CustomerSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Customers.AnyAsync() == false)
            {
                var customers = await SeedHelper.GetSeedDataFromJson<Customer>("CustomerSeed.json");
                await dbContext.Customers.AddRangeAsync(customers);
            }
        }
    }
}
