namespace GarageBuddy.Data.Seeding.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;

    using Job;

    using Newtonsoft.Json;

    public static class SeedHelper
    {
        private static readonly string SeedPath = Path.Combine("Seeding", "Data");

        public static async Task<List<TEntity>> GetSeedDataFromJson<TEntity>(string fileName)
        {
            var fullPath = GetJsonFromSeedFile(fileName);
            var json = await File.ReadAllTextAsync(fullPath);
            var result = JsonConvert.DeserializeObject<List<TEntity>>(json)
                         ?? throw new InvalidOperationException(string.Format(Errors.DeserializationFailed, fullPath));

            return result;
        }

        private static string GetJsonFromSeedFile(string fileName)
        {
            var assembly = Assembly.GetAssembly(typeof(JobStatusSeeder)) ?? throw new InvalidOperationException("Invalid seed assembly.");
            var currentDirectory = Path.GetDirectoryName(assembly.Location)
                                   ?? throw new InvalidOperationException(Errors.InvalidDirectoryPath);
            return Path.Combine(currentDirectory, SeedPath, fileName);
        }
    }
}
