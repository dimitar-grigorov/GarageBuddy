using System.Reflection;

using GarageBuddy.Common.Constants;
using GarageBuddy.Data;
using GarageBuddy.Data.Models;
using GarageBuddy.Services.Data.Options;
using GarageBuddy.Services.Mapping;
using GarageBuddy.Web.Infrastructure.Extensions;
using GarageBuddy.Web.ViewModels;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(nameof(ConnectionStringsOptions.DefaultConnection)); //?? throw new InvalidOperationException("Connection string not found");

if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = "Error";
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<ConnectionStringsOptions>(
    builder.Configuration.GetSection(ConnectionStringsOptions.ConnectionStrings));

builder.Services.ConfigureCookiePolicy();
builder.Services.ConfigureApplicationCookie();

builder.Services.AddControllersWithViews()
    .AddMvcOptions(options =>
    {
        // TODO: Add model binder providers
        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    });

builder.Services.AddRazorPages();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddDataRepositories();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Seed data on application startup
/*using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}*/

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

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

// Added to fix Web.Tests project
public partial class Program
{
}
