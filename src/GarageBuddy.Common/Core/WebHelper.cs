namespace GarageBuddy.Common.Core
{
    using System;
    using System.Linq;
    using System.Net;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// Represents a web helper.
    /// </summary>
    public partial class WebHelper : IWebHelper
    {
        #region Fields

        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHostApplicationLifetime hostApplicationLifetime;

        #endregion

        #region Ctor

        public WebHelper(IHttpContextAccessor httpContextAccessor,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.hostApplicationLifetime = hostApplicationLifetime;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets this page URL.
        /// </summary>
        /// <param name="includeQueryString">Value indicating whether to include query strings.</param>
        /// <param name="useSsl">Value indicating whether to get SSL secured page URL. Pass null to determine automatically.</param>
        /// <param name="lowercaseUrl">Value indicating whether to lowercase URL.</param>
        /// <returns>Page URL.</returns>
        public virtual string GetThisPageUrl(bool includeQueryString, bool? useSsl = null, bool lowercaseUrl = false)
        {
            if (!IsRequestAvailable())
            {
                return string.Empty;
            }

            // get store location
            var storeLocation = GetStoreLocation(useSsl ?? IsCurrentConnectionSecured());

            // add local path to the URL
            var pageUrl = $"{storeLocation.TrimEnd('/')}{httpContextAccessor.HttpContext.Request.Path}";

            // add query string to the URL
            if (includeQueryString)
            {
                pageUrl = $"{pageUrl}{httpContextAccessor.HttpContext.Request.QueryString}";
            }

            // whether to convert the URL to lower case
            if (lowercaseUrl)
            {
                pageUrl = pageUrl.ToLowerInvariant();
            }

            return pageUrl;
        }

        /// <summary>
        /// Gets a value indicating whether current connection is secured.
        /// </summary>
        /// <returns>True if it's secured, otherwise false.</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            if (!IsRequestAvailable())
            {
                return false;
            }

            return httpContextAccessor.HttpContext.Request.IsHttps;
        }

        /// <summary>
        /// Gets store host location.
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL.</param>
        /// <returns>Store host location.</returns>
        public virtual string GetStoreHost(bool useSsl)
        {
            if (!IsRequestAvailable())
            {
                return string.Empty;
            }

            // try to get host from the request HOST header
            var hostHeader = httpContextAccessor.HttpContext.Request.Headers["Host"];
            if (StringValues.IsNullOrEmpty(hostHeader))
            {
                return string.Empty;
            }

            // add scheme to the URL
            var storeHost = $"{(useSsl ? Uri.UriSchemeHttps : Uri.UriSchemeHttp)}{Uri.SchemeDelimiter}{hostHeader.FirstOrDefault()}";

            // ensure that host is ended with slash
            storeHost = $"{storeHost.TrimEnd('/')}/";

            return storeHost;
        }

        /// <summary>
        /// Gets store location.
        /// </summary>
        /// <param name="useSsl">Whether to get SSL secured URL; pass null to determine automatically.</param>
        /// <returns>Store location.</returns>
        public virtual string GetStoreLocation(bool? useSsl = null)
        {
            var storeLocation = string.Empty;

            // get store host
            var storeHost = GetStoreHost(useSsl ?? IsCurrentConnectionSecured());
            if (!string.IsNullOrEmpty(storeHost))
            {
                // add application path base if exists
                storeLocation = IsRequestAvailable() ? $"{storeHost.TrimEnd('/')}{httpContextAccessor.HttpContext.Request.PathBase}" : storeHost;
            }

            // ensure that URL is ended with slash
            storeLocation = $"{storeLocation.TrimEnd('/')}/";

            return storeLocation;
        }

        /// <summary>
        /// Restart application domain.
        /// </summary>
        public virtual void RestartAppDomain()
        {
            hostApplicationLifetime.StopApplication();
        }

        /// <summary>
        /// Gets current HTTP request protocol.
        /// </summary>
        public virtual string GetCurrentRequestProtocol()
        {
            return IsCurrentConnectionSecured() ? Uri.UriSchemeHttps : Uri.UriSchemeHttp;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Check whether current HTTP request is available.
        /// </summary>
        /// <returns>True if available; otherwise false.</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (httpContextAccessor?.HttpContext == null)
            {
                return false;
            }

            try
            {
                if (httpContextAccessor.HttpContext?.Request == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is IP address specified.
        /// </summary>
        /// <param name="address">IP address.</param>
        /// <returns>Result.</returns>
        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            var rez = address != null && address.ToString() != IPAddress.IPv6Loopback.ToString();

            return rez;
        }

        #endregion
    }
}
