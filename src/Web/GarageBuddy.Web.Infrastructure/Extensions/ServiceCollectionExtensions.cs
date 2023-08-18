// namespace Microsoft.Extensions.DependencyInjection
namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Data;
    using Data.DataProvider;
    using Data.Repositories;

    using DataTables.AspNet.AspNetCore;

    using GarageBuddy.Common.Constants;
    using GarageBuddy.Common.Core;
    using GarageBuddy.Common.Core.Settings;
    using GarageBuddy.Common.Core.Settings.Mail;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Services.Data.Contracts;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using Serilog;

    using Services.Data.Options;
    using Services.Messaging.Contracts;
    using Services.Messaging.Email;
    using Services.Messaging.Services;

    using ViewRenderer;

    using ILogger = Serilog.ILogger;

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
                .AddApplicationServices(typeof(IBrandModelService))
                .AddDatabaseDeveloperPageExceptionFilter();
        }

        public static IServiceCollection ConfigureCookiePolicy(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.Strict;
                    options.Secure = CookieSecurePolicy.Always;
                });

            return services;
        }

        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = GlobalConstants.UserLoginRoute;
                options.LogoutPath = GlobalConstants.UserLogoutRoute;
                options.AccessDeniedPath = $"{GlobalConstants.ErrorRoute}/401";
            });

            return services;
        }

        public static IServiceCollection AddDataTables(this IServiceCollection services)
        {
            var options = new DataTables.AspNet.AspNetCore.Options()
                .EnableRequestAdditionalParameters()
                .EnableResponseAdditionalParameters();

            var binder = new ModelBinder
            {
                ParseAdditionalParameters = context =>
                {
                    var includeDeleted = Convert.ToBoolean(context.ValueProvider.GetValue(GlobalConstants.IncludeDeletedFilterName).FirstValue);
                    var id = context.ValueProvider.GetValue(GlobalConstants.IdFilterName).FirstValue ?? string.Empty;
                    return new Dictionary<string, object>()
                    {
                        { GlobalConstants.IncludeDeletedFilterName, includeDeleted },
                        { GlobalConstants.IdFilterName, id },
                    };
                },
            };

            services.RegisterDataTables(options, binder);

            return services;
        }

        internal static IServiceCollection AddApplicationServices(this IServiceCollection services, Type serviceType)
        {
            Assembly? serviceAssembly = Assembly.GetAssembly(serviceType);
            if (serviceAssembly == null)
            {
                throw new InvalidOperationException("Invalid service type provided!");
            }

            Type[] implementationTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Service") && !t.IsInterface)
                .ToArray();
            foreach (Type implementationType in implementationTypes)
            {
                Type? interfaceType = implementationType
                    .GetInterface($"I{implementationType.Name}");
                if (interfaceType == null)
                {
                    throw new InvalidOperationException(
                        $"No interface is provided for the service with name: {implementationType.Name}");
                }

                services.AddScoped(interfaceType, implementationType);
            }

            // Extra services
            services.AddScoped<IViewRenderer, ViewRenderer>();
            services.AddTransient<IEmailSender, SmtpMailSender>();
            services.AddTransient<IEmailService, EmailService>();

            // Options manager
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
                    m.UseDatabase(databaseSettings.DbProvider, databaseSettings.DefaultConnection);
                })
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
            switch (dbProvider.ToLowerInvariant())
            {
                case DbProviderKeys.SqlServer:
                    {
                        return builder.UseSqlServer(connectionString,
                                e => e.MigrationsAssembly("GarageBuddy.Data"))
                            .UseLoggerFactory(LoggerFactory.Create(b => b.AddSerilog()));
                    }
                default:
                    throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
            }
        }
    }
}
