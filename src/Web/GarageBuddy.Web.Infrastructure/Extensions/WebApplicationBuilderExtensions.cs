namespace GarageBuddy.Web.Infrastructure.Extensions
{
    using GarageBuddy.Data;
    using GarageBuddy.Data.Common;
    using GarageBuddy.Data.Common.Repositories;
    using GarageBuddy.Data.Repositories;
    using GarageBuddy.Services.Data;
    using GarageBuddy.Services.Messaging;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public static class WebApplicationBuilderExtensions
    {
        public static IServiceCollection AddDataRepositories(this IServiceCollection services)
        {
            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // TODO: This is for Raw SQL queries - remove if not needed
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();

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
    }
}
