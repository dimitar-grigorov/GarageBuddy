namespace GarageBuddy.Data.DataProvider
{
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core;

    /// <summary>
    /// Represents a data provider.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Create the database.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <param name="triesToConnect">Count of tries to connect to the database after creating;
        /// set 0 if no need to connect after creating.</param>
        public void CreateDatabase(string connectionString, int triesToConnect = 10);

        /// <summary>
        /// Checks if the specified database exists, returns true if database exists.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the returns true if the database exists.
        /// </returns>
        Task<bool> DatabaseExistsAsync(string connectionString);

        /// <summary>
        /// Checks if the specified database exists, returns true if database exists.
        /// </summary>
        /// <returns>Returns true if the database exists.</returns>
        bool DatabaseExists(string connectionString);

        /// <summary>
        /// Build the connection string.
        /// </summary>
        /// <param name="connectionString">Connection string info.</param>
        /// <returns>Connection string.</returns>
        string BuildConnectionString(IConnectionStringInfo connectionString);
    }
}
