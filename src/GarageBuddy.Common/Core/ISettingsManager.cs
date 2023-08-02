namespace GarageBuddy.Common.Core
{
    public interface ISettingsManager
    {
        /// <summary>
        /// Gets a value indicating whether database is already installed.
        /// </summary>
        public bool IsDatabaseInstalled();
    }
}
