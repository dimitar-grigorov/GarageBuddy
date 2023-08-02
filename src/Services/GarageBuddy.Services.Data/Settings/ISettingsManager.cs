namespace GarageBuddy.Services.Data.Settings
{
    public interface ISettingsManager
    {
        /// <summary>
        /// Gets a value indicating whether database is already installed.
        /// </summary>
        public bool IsDatabaseInstalled();
    }
}
