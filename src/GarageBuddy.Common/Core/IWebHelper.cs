namespace GarageBuddy.Common.Core
{
    /// <summary>
    /// Represents a web helper.
    /// </summary>
    public interface IWebHelper
    {
        /// <summary>
        /// Gets this page URL.
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings.</param>
        /// <param name="useSsl">Value indicating whether to get SSL secured page URL. Pass null to determine automatically.</param>
        /// <param name="lowercaseUrl">Value indicating whether to lowercase URL.</param>
        /// <returns>Page URL.</returns>
        string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false);

        /// <summary>
        /// Gets a value indicating whether current connection is secured.
        /// </summary>
        /// <returns>True if it's secured, otherwise false.</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// Gets store host location.
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL.</param>
        /// <returns>Store host location.</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// Gets store location.
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL; pass null to determine automatically.</param>
        /// <returns>Store location.</returns>
        string GetStoreLocation(bool? useSsl = null);

        /// <summary>
        /// Restart application domain.
        /// </summary>
        void RestartAppDomain();

        /// <summary>
        /// Gets current HTTP request protocol.
        /// </summary>
        string GetCurrentRequestProtocol();
    }
}
