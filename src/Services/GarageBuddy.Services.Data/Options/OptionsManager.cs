namespace GarageBuddy.Services.Data.Options
{
    using Microsoft.Extensions.Options;

    public class OptionsManager : IOptionsManager
    {
        /// <summary>
        /// Gets a cached value indicating whether the database is installed. We need this value invariable during installation process.
        /// </summary>
        private static bool? databaseIsInstalled;

        private readonly IOptions<ConnectionStringsOptions> connectionStringsOptions;

        public OptionsManager(IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            this.connectionStringsOptions = connectionStringsOptions;
        }

        public bool IsDatabaseInstalled()
        {
            databaseIsInstalled ??= !string.IsNullOrEmpty(connectionStringsOptions.Value.DefaultConnection);
            return databaseIsInstalled.Value;
        }
    }
}
