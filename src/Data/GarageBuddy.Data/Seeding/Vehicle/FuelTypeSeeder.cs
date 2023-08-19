namespace GarageBuddy.Data.Seeding.Vehicle
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle;

    public class FuelTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.FuelTypes.AnyAsync() == false)
            {
                var fuelTypes = await SeedHelper.GetSeedDataFromJson<string>("FuelTypeSeed.json");

                foreach (var driveType in fuelTypes)
                {
                    await dbContext.FuelTypes.AddAsync(new FuelType()
                    {
                        FuelName = driveType,
                    });
                }
            }
        }
    }
}
