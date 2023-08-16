namespace GarageBuddy.Data.Seeding.Vehicle
{
    using System;
    using System.Threading.Tasks;

    using GarageBuddy.Data.Seeding.Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Vehicle;

    public class DriveTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.DriveTypes.AnyAsync() == false)
            {
                var driveTypes = await SeedHelper.GetSeedDataFromJson<string>("DriveTypeSeed.json");

                foreach (var driveType in driveTypes)
                {
                    await dbContext.DriveTypes.AddAsync(new DriveType
                    {
                        DriveTypeName = driveType,
                    });
                }
            }
        }
    }
}
