namespace GarageBuddy.Data.Seeding.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using GarageBuddy.Data.Seeding.Job;

    using Newtonsoft.Json;

    public static class SeedHelper
    {
        private const string SeedPath = "Seeding\\Data";

        public static async Task<List<TEntity>> GetSeedDataFromJson<TEntity>(string fileName)
        {
            // Get the directory path from the assembly location
            var currentDirectory = Path.GetDirectoryName(Assembly.GetAssembly(typeof(JobStatusSeeder)).Location)
                                   ?? throw new InvalidOperationException(Errors.InvalidDirectoryPath);
            var fullPath = Path.Combine(currentDirectory, SeedPath, fileName);

            var json = await File.ReadAllTextAsync(fullPath);
            var result = JsonConvert.DeserializeObject<List<TEntity>>(json)
                         ?? throw new InvalidOperationException(string.Format(Errors.DeserializationFailed, fullPath));

            return result;
        }
    }
}
