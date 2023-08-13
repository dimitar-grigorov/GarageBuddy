namespace GarageBuddy.Services.Data.Installation
{
    using System;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Core;
    using Microsoft.AspNetCore.Http;

    using Options;

    /// <summary>
    /// Represents middleware that checks whether database is installed and redirects to installation URL in otherwise.
    /// </summary>
    public class InstallUrlMiddleware
    {
        private readonly RequestDelegate next;

        public InstallUrlMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        /// <summary>
        /// Invoke middleware actions.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <param name="webHelper">Web helper.</param>
        /// <param name="optionsManager">Settings manager.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context, IWebHelper webHelper, IOptionsManager optionsManager)
        {
            // whether database is installed
            if (!optionsManager.IsDatabaseInstalled())
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
            if (this.next == null)
            {
                return;
            }

            await next(context);
        }
    }
}
