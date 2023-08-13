// namespace Microsoft.Extensions.DependencyInjection
namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using System;

    using Data.DataProvider;

    using GarageBuddy.Common.Constants;
    using GarageBuddy.Common.Core;
    using GarageBuddy.Common.Core.Settings;
    using GarageBuddy.Common.Core.Settings.Mail;
    using GarageBuddy.Data;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Repositories;
    using GarageBuddy.Services.Data.Contracts;
    using GarageBuddy.Services.Data.Services;
    using GarageBuddy.Services.Messaging.Email;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Serilog;

    using Services.Data.Options;
    using Services.Messaging.Contracts;
    using Services.Messaging.Services;

    using ViewRenderer;

    /// <summary>
    /// Represents extensions of IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        private static readonly ILogger Logger = Log.ForContext(typeof(ServiceCollectionExtensions));

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            // TODO: Remove Razor Pages
            services.AddRazorPages();
            return services
                .AddPersistence()
                .AddApplicationServices()
                .AddDatabaseDeveloperPageExceptionFilter();
        }

        public static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            return services;
        }

        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/User/Logout";
                options.AccessDeniedPath = $"{GlobalConstants.ErrorRoute}/401";
            });

            return services;
        }

        internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application services
            services.AddScoped<IViewRenderer, ViewRenderer>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddTransient<IEmailSender, SmtpMailSender>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IInstallationService, InstallationService>();

            // Web helper
            services.AddScoped<IWebHelper, WebHelper>();

            // Settings manager
            services.AddSingleton<IOptionsManager, OptionsManager>();

            return services;
        }

        internal static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddOptions<DatabaseSettings>()
                .BindConfiguration(nameof(DatabaseSettings))
                .PostConfigure(databaseSettings =>
                {
                    Logger.Information("Current DB Provider: {dbProvider}", databaseSettings.DbProvider);
                })
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailSettings>()
                .BindConfiguration(nameof(EmailSettings))
                .ValidateDataAnnotations();

            return services
                .AddDbContext<ApplicationDbContext>((p, m) =>
                {
                    var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
                    m.UseDatabase(databaseSettings.DbProvider, databaseSettings.ConnectionString);
                })
                /*.AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddTransient<ApplicationDbInitializer>()
                .AddTransient<ApplicationDbSeeder>()
                .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
                .AddTransient<CustomSeederRunner>()

                .AddTransient<IConnectionStringSecurer, ConnectionStringSecurer>()
                .AddTransient<IConnectionStringValidator, ConnectionStringValidator>()*/

                .AddRepositories();
        }

        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<,>), typeof(EfDeletableEntityRepository<,>));
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
            services.AddScoped<IDataProvider, MsSqlDataProvider>();

            return services;
        }

        private static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            return dbProvider.ToLowerInvariant() switch
            {
                DbProviderKeys.SqlServer => builder.UseSqlServer(connectionString, e =>
                    e.MigrationsAssembly("GarageBuddy.Data")),
                _ => throw new InvalidOperationException($"DB Provider {dbProvider} is not supported."),
            };
        }
    }
}
