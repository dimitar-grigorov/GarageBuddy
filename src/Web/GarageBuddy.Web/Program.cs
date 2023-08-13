using System;
using System.Reflection;

using DataTables.AspNet.AspNetCore;

using GarageBuddy.Common.Constants;
using GarageBuddy.Data;
using GarageBuddy.Data.Models;
using GarageBuddy.Data.Seeding;
using GarageBuddy.Services.Data.Models.Vehicle.Brand;
using GarageBuddy.Services.Mapping;
using GarageBuddy.Web.Configurations;
using GarageBuddy.Web.Infrastructure.Common;
using GarageBuddy.Web.Infrastructure.Extensions;
using GarageBuddy.Web.ViewModels;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

StaticLogger.EnsureInitialized();
Log.Information("Server starting ...");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.AddConfigurations().RegisterSerilog();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.RegisterDataTables();

    builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>();

    builder.Services.ConfigureCookiePolicy();
    builder.Services.ConfigureApplicationCookie();

    builder.Services.AddControllersWithViews()
        .AddMvcOptions(options => // TODO: Add model binder providers
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

    // AutoMapper configuration
    var assemblies = new[]
    {
        typeof(ErrorViewModel).GetTypeInfo().Assembly,
        typeof(BrandServiceModel).GetTypeInfo().Assembly,
    };
    AutoMapperConfig.RegisterMappings(assemblies);
    builder.Services.AddSingleton(AutoMapperConfig.MapperInstance);

    var app = builder.Build();

    // Seed data on application startup
    using (var serviceScope = app.Services.CreateScope())
    {
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler($"{GlobalConstants.ErrorRoute}/500");
        app.UseHsts();
    }

    app.UseStatusCodePagesWithRedirects($"{GlobalConstants.ErrorRoute}/?statusCode={{0}}");
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseCookiePolicy();
    app.UseRouting();
    app.UseInstallUrl();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("StopTheHostException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("Server stopping...");
    Log.CloseAndFlush();
}

// Added to fix Web.Tests project
public partial class Program
{
}
