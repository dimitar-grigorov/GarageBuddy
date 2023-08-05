namespace GarageBuddy.Data.DataProvider
{
    using System.Data.Common;

    public abstract class BaseDataProvider
    {
        /// <summary>
        /// Gets a connection to the database for a current data provider.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Connection to a database.</returns>
        protected abstract DbConnection GetInternalDbConnection(string connectionString);
    }
}
