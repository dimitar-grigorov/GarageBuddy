﻿namespace GarageBuddy.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using GarageBuddy.Common.Constants;
    using GarageBuddy.Data.Models;
    using GarageBuddy.Data.Seeding.Common;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            await SeedRoleAsync(roleManager, GlobalConstants.Roles.Administrator);
            await SeedRoleAsync(roleManager, GlobalConstants.Roles.Manager);
            await SeedRoleAsync(roleManager, GlobalConstants.Roles.Mechanic);
            await SeedRoleAsync(roleManager, GlobalConstants.Roles.User);
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
