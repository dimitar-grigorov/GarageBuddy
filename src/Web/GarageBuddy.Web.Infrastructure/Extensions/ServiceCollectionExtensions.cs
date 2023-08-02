namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using GarageBuddy.Common.Core;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Repositories;
    using GarageBuddy.Services.Data.Contracts;
    using GarageBuddy.Services.Data.Services;
    using GarageBuddy.Services.Messaging;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Represents extensions of IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<,>), typeof(EfDeletableEntityRepository<,>));
            services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IUserService, UserService>();

            // Web helper
            services.AddScoped<IWebHelper, WebHelper>();

            // Settings manager
            services.AddSingleton<ISettingsManager, SettingsManager>();

            return services;
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
                options.AccessDeniedPath = "/User/AccessDenied";
            });

            return services;
        }
    }
}
