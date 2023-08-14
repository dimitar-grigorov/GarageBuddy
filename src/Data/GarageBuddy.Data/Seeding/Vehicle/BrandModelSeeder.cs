namespace GarageBuddy.Data.Seeding.Vehicle
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle;

    public class BrandModelSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.BrandModels.AnyAsync() == false)
            {
                var models = await SeedHelper.GetSeedDataFromJson<BrandModel>("BrandModelSeed.json");
                dbContext.BrandModels.AddRange(models);
            }
        }
    }
}
