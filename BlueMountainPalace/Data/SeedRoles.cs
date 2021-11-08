using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueMountainPalace.Data
{
    public class SeedRoles
    {
        // Create and update the role table
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {

            // Get the RoleManager and the UserManager objects
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Include the role names
            string[] roleNames = { "Administrator", "Customer" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                // Check whether the role already exists in the role table
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // If the role does not exist, create the role
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create an admin user
            // the username/password are read from the configuration file: appsettings.json
            var poweruser = new IdentityUser
            {
                UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                Email = Configuration.GetSection("UserSettings")["UserEmail"]
            };
            string userPassword = Configuration.GetSection("UserSettings")["UserPassword"];

            // Check whether the username(email) exists in the database
            var user = await UserManager.FindByEmailAsync(Configuration.GetSection("UserSettings")["UserEmail"]);

            // If the admin does not exist in the database, create it in the database;
            // Otherwise, pass
            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    // Assign the new user the "Administrator" role
                    await UserManager.AddToRoleAsync(poweruser, "Administrator");
                }
            }
        }
    }
}
