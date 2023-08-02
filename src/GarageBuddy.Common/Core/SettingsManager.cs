namespace GarageBuddy.Common.Core
{
    public class SettingsManager : ISettingsManager
    {
        /// <summary>
        /// Gets a cached value indicating whether the database is installed. We need this value invariable during installation process.
        /// </summary>
        private static bool? databaseIsInstalled;

        public bool IsDatabaseInstalled()
        {
            // databaseIsInstalled ??= !string.IsNullOrEmpty(LoadSettings()?.ConnectionString);
            // TODO: Implement this method
            databaseIsInstalled = true;
            return databaseIsInstalled.Value;
        }
    }
}
