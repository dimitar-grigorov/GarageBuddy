namespace GarageBuddy.Data.Seeding.Vehicle
{
    using System;
    using System.Threading.Tasks;

    using Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle;

    public class GearboxTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.GearboxTypes.AnyAsync() == false)
            {
                var gearboxTypes = await SeedHelper.GetSeedDataFromJson<string>("GearboxTypeSeed.json");

                foreach (var gearboxType in gearboxTypes)
                {
                    await dbContext.GearboxTypes.AddAsync(new GearboxType()
                    {
                        GearboxTypeName = gearboxType,
                    });
                }
            }
        }
    }
}
