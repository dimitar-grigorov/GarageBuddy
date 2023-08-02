namespace GarageBuddy.Services.Data.Installation
{
    using System;
    using System.Threading.Tasks;

    using Common.Core;

    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Represents middleware that checks whether database is installed and redirects to installation URL in otherwise.
    /// </summary>
    public class InstallUrlMiddleware
    {
        #region Fields

        private readonly RequestDelegate next;

        #endregion

        #region Ctor

        public InstallUrlMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke middleware actions.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <param name="webHelper">Web helper.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, IWebHelper webHelper, ISettingsManager settingsManager)
        {
            // whether database is installed
            if (!settingsManager.IsDatabaseInstalled())
            {
                var installUrl = $"{webHelper.GetStoreLocation()}{GarageInstallationDefaults.InstallPath}";
                if (!webHelper.GetThisPageUrl(false).StartsWith(installUrl, StringComparison.InvariantCultureIgnoreCase))
                {
                    // redirect
                    context.Response.Redirect(installUrl);
                    return;
                }
            }

            // or call the next middleware in the request pipeline
            await next(context);
        }

        #endregion
    }
}
