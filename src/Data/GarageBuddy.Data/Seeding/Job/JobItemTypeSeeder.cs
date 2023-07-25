namespace GarageBuddy.Data.Seeding.Job
{
    using System;
    using System.Threading.Tasks;

    using GarageBuddy.Data.Seeding.Common;

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
