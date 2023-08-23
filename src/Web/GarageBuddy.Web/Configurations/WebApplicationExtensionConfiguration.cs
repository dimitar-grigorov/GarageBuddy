namespace GarageBuddy.Web.Configurations
{
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    internal static class WebApplicationExtensionConfiguration
    {
        internal static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
        {
            // TODO: Only optional files should be these including {env.EnvironmentName}
            const string configurationsDirectory = "Configurations";
            var env = builder.Environment;
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/logger.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/logger.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/hangfire.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/hangfire.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cache.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cache.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cors.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/cors.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/database.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/mail.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/mail.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/middleware.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/middleware.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/security.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/security.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/openapi.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/openapi.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/signalr.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/signalr.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/securityheaders.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/securityheaders.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/localization.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/localization.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets(Assembly.GetExecutingAssembly())
                    .AddEnvironmentVariables();
            return builder;
        }
    }
}
