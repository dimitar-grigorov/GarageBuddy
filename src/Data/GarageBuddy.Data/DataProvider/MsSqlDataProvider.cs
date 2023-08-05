namespace GarageBuddy.Data.DataProvider
{
    using System;
    using System.Data.Common;
    using System.Threading;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core;

    using Microsoft.Data.SqlClient;

    /// <summary>
    /// Represents the MS SQL Server data provider.
    /// </summary>
    public class MsSqlDataProvider : BaseDataProvider, IDataProvider
    {
        public void CreateDatabase(string connectionString, int triesToConnect = 10)
        {
            if (DatabaseExists(connectionString))
            {
                return;
            }

            var builder = new SqlConnectionStringBuilder(connectionString);

            // Gets database name
            var databaseName = builder.InitialCatalog;

            // Now create connection string to 'master' database. It always exists.
            builder.InitialCatalog = "master";

            using (var connection = GetInternalDbConnection(builder.ConnectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "CREATE DATABASE [@name]";
                command.Parameters.Add(new SqlParameter("@name", databaseName));
                command.Connection?.Open();

                command.ExecuteNonQuery();
            }

            // try connect
            if (triesToConnect <= 0)
            {
                return;
            }

            // sometimes on slow servers (hosting) there could be situations when database requires some time to be created.
            // but we have already started creation of tables and sample data.
            // as a result there is an exception thrown and the installation process cannot continue.
            // that's why we are in a cycle of "triesToConnect" times trying to connect to a database with a delay of one second.
            for (var i = 0; i <= triesToConnect; i++)
            {
                if (i == triesToConnect)
                {
                    throw new Exception("Unable to connect to the new database. Please try one more time");
                }

                if (!DatabaseExists(connectionString))
                {
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
            }
        }

        public async Task<bool> DatabaseExistsAsync(string connectionString)
        {
            try
            {
                await using var connection = GetInternalDbConnection(connectionString);

                // just try to connect
                await connection.OpenAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DatabaseExists(string connectionString)
        {
            try
            {
                using var connection = GetInternalDbConnection(connectionString);

                // just try to connect
                connection.Open();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Build the connection string.
        /// </summary>
        /// <param name="connectionString">Connection string info.</param>
        /// <returns>Connection string.</returns>
        public virtual string BuildConnectionString(IConnectionStringInfo connectionString)
        {
            if (connectionString is null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = connectionString.ServerName,
                InitialCatalog = connectionString.DatabaseName,
                PersistSecurityInfo = false,
                IntegratedSecurity = connectionString.IntegratedSecurity,
                TrustServerCertificate = true,
            };

            if (!connectionString.IntegratedSecurity)
            {
                builder.UserID = connectionString.Username;
                builder.Password = connectionString.Password;
            }

            return builder.ConnectionString;
        }

        protected override DbConnection GetInternalDbConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            return new SqlConnection(connectionString);
        }
    }
}
