namespace GarageBuddy.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using GarageBuddy.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class GarageSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Garages.AnyAsync() == false)
            {
                var garages = await SeedHelper.GetSeedDataFromJson<Garage>("GarageSeed.json");
                await dbContext.Garages.AddRangeAsync(garages);
            }
        }
    }
}
