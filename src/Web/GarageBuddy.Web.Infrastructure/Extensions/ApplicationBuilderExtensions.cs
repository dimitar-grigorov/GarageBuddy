namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using GarageBuddy.Services.Data.Installation;

    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Represents extensions of IApplicationBuilder.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure middleware checking whether database is installed.
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline.</param>
        public static void UseInstallUrl(this IApplicationBuilder application)
        {
            application.UseMiddleware<InstallUrlMiddleware>();
        }
    }
}
