using EarlyAlertV2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EarlyAlertV2.Repository.SeedData
{
    public static class Seeder
    {
        public static void SeedIt(IServiceProvider serviceProvider, string adminUserName, string adminPassword)
        {
            SeedAccount(serviceProvider, adminUserName, adminPassword).Wait();
        }

        private static async Task SeedAccount(IServiceProvider serviceProvider, string adminUserName, string adminPassword)
        {

            IServiceScopeFactory scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                // Create roles
                var identityRoles = new List<IdentityRole>
                {
                    new IdentityRole{ Id = UserRoles.OITAdminRoleId, Name = UserRoles.OITAdminRole, NormalizedName = UserRoles.OITAdminRole.ToUpper() },
                    new IdentityRole{ Id = UserRoles.AdminRoleId, Name = UserRoles.AdminRole, NormalizedName = UserRoles.AdminRole.ToUpper() },
                };

                foreach (var role in identityRoles)
                {
                    // Create the Admin Role
                    var roleExist = await roleManager.RoleExistsAsync(role.Name);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                //Create Users
                var adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminUserName,
                    FirstName = "OIT",
                    LastName = "Admin",
                };

                await CreateUserAndRole(userManager, adminUser, adminPassword, UserRoles.OITAdminRole);
            }
        }

        private static async Task CreateUserAndRole(UserManager<ApplicationUser> mgr, ApplicationUser appUser, string password, string roleName)
        {
            var user = await mgr.FindByEmailAsync(appUser.Email);
            if (user == null)
            {
                var createUser = await mgr.CreateAsync(appUser, password);
                if (createUser.Succeeded)
                {
                    await mgr.AddToRoleAsync(appUser, roleName);
                }
            }
        }
    }
}
