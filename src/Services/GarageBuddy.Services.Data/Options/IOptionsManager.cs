namespace GarageBuddy.Services.Data.Options
{
    public interface IOptionsManager
    {
        /// <summary>
        /// Gets a value indicating whether database is already installed.
        /// </summary>
        public bool IsDatabaseInstalled();
    }
}
