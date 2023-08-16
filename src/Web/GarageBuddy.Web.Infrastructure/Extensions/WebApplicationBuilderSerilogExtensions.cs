namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using GarageBuddy.Common.Core.Settings;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Formatting.Compact;

    public static class WebApplicationBuilderSerilogExtensions
    {
        public static void RegisterSerilog(this WebApplicationBuilder builder)
        {
            builder.Services.AddOptions<LoggerSettings>().BindConfiguration(nameof(LoggerSettings));

            _ = builder.Host.UseSerilog((_, sp, logConfig) =>
            {
                LoggerSettings loggerSettings = sp.GetRequiredService<IOptions<LoggerSettings>>().Value;
                string appName = loggerSettings.AppName;
                string elasticSearchUrl = loggerSettings.ElasticSearchUrl;
                bool writeToFile = loggerSettings.WriteToFile;
                bool structuredConsoleLogging = loggerSettings.StructuredConsoleLogging;
                string minLogLevel = loggerSettings.MinimumLogLevel;
                ConfigureEnrichers(logConfig, appName);
                ConfigureConsoleLogging(logConfig, structuredConsoleLogging);
                ConfigureWriteToFile(logConfig, writeToFile);

                SetMinimumLogLevel(logConfig, minLogLevel);
                OverrideMinimumLogLevel(logConfig);
            });
        }

        private static void ConfigureEnrichers(LoggerConfiguration logConfig, string appName)
        {
            logConfig
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithExceptionDetails();
        }

        private static void ConfigureConsoleLogging(LoggerConfiguration logConfig, bool structuredConsoleLogging)
        {
            if (structuredConsoleLogging)
            {
                logConfig.WriteTo.Async(wt => wt.Console(new CompactJsonFormatter()));
            }
            else
            {
                logConfig.WriteTo.Async(wt => wt.Console());
            }
        }

        private static void ConfigureWriteToFile(LoggerConfiguration logConfig, bool writeToFile)
        {
            if (writeToFile)
            {
                logConfig.WriteTo.File(
                    new CompactJsonFormatter(),
                    "Logs/logs.json",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 5);
            }
        }

        private static void OverrideMinimumLogLevel(LoggerConfiguration logConfig)
        {
            logConfig
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error);
        }

        private static void SetMinimumLogLevel(LoggerConfiguration logConfig, string minLogLevel)
        {
            switch (minLogLevel.ToLower())
            {
                case "debug":
                    logConfig.MinimumLevel.Debug();
                    break;
                case "information":
                    logConfig.MinimumLevel.Information();
                    break;
                case "warning":
                    logConfig.MinimumLevel.Warning();
                    break;
                default:
                    logConfig.MinimumLevel.Information();
                    break;
            }
        }
    }
}
