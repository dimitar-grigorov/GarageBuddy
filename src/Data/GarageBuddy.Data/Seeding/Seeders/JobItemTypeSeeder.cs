namespace GarageBuddy.Data.Seeding.Seeders
{
    using System;
    using System.Threading.Tasks;

    using Models.Job;

    public class JobItemTypeSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var jobs = await SeedHelper.GetSeedDataFromJson<JobItemType>("JobItemTypeSeed.json");

            foreach (var item in jobs)
            {
                await dbContext.JobItemTypes.AddAsync(item);
            }
        }
    }
}
