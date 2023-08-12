namespace GarageBuddy.Data.Seeding.Job
{
    using System;
    using System.Threading.Tasks;

    using Extensions;

    using GarageBuddy.Data.Seeding.Common;

    using Microsoft.EntityFrameworkCore;

    using Models.Enums;
    using Models.Job;

    public class JobStatusSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.JobStatus.CountAsync() < Enum.GetNames(typeof(JobStatusEnum)).Length)
            {
                dbContext.JobStatus.SeedEnumValues<JobStatus, JobStatusEnum>(@enum => @enum);
            }
        }
    }
}
