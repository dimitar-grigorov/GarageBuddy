namespace GarageBuddy.Services.Data.Options
{
    using GarageBuddy.Common.Core.Settings;

    using Microsoft.Extensions.Options;

    public class OptionsManager : IOptionsManager
    {
        /// <summary>
        /// Gets a cached value indicating whether the database is installed. We need this value invariable during installation process.
        /// </summary>
        private static bool? databaseIsInstalled;

        private readonly IOptions<DatabaseSettings> connectionStringsOptions;

        public OptionsManager(IOptions<DatabaseSettings> connectionStringsOptions)
        {
            this.connectionStringsOptions = connectionStringsOptions;
        }

        public bool IsDatabaseInstalled()
        {
            databaseIsInstalled ??= !string.IsNullOrEmpty(connectionStringsOptions.Value.ConnectionString);
            return databaseIsInstalled.Value;
        }
    }
}
