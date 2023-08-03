namespace GarageBuddy.Common.Core
{
    /// <summary>
    /// Represents a connection string info.
    /// </summary>
    public interface IConnectionStringInfo
    {
        /// <summary>
        /// Gets or sets Database name.
        /// </summary>
        string DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets Server name or IP address.
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether
        /// Integrated security is enabled.
        /// </summary>
        bool IntegratedSecurity { get; set; }

        /// <summary>
        /// Gets or sets Username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets or sets Password.
        /// </summary>
        string Password { get; set; }
    }
}
