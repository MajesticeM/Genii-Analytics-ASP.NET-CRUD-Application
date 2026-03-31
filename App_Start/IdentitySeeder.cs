using System;
using Genii_Assessment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Genii_Assessment.App_Start
{
    /// <summary>
    /// Seeds default application roles and an administrator account.
    /// </summary>
    public static class IdentitySeeder
    {
        /// <summary>
        /// Creates required roles and a default administrator user if they do not already exist.
        /// This method is safe to call multiple times.
        /// </summary>
        public static void SeedRolesAndAdmin()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

                var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

                SeedRoles(roleManager);
                SeedAdminUser(userManager);
            }
        }

        /// <summary>
        /// Ensures that the required system roles exist.
        /// </summary>
        /// <param name="roleManager">Identity role manager.</param>
        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = {
                ApplicationRoles.Admin,
                ApplicationRoles.User,
                ApplicationRoles.Manager
            };

            foreach (var role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new IdentityRole(role));
                }
            }
        }

        /// <summary>
        /// Ensures that a default administrator account exists and belongs to the Admin role.
        /// </summary>
        /// <param name="userManager">Identity user manager.</param>
        private static void SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
            const string adminEmail = "admin@geniiassessment.local";
            const string adminPassword = "Admin@12345";

            var adminUser = userManager.FindByName(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var createResult = userManager.Create(adminUser, adminPassword);

                if (createResult.Succeeded)
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }
            }
            else
            {
                if (!userManager.IsInRole(adminUser.Id, "Admin"))
                {
                    userManager.AddToRole(adminUser.Id, "Admin");
                }

                if (!adminUser.IsActive)
                {
                    adminUser.IsActive = true;
                    userManager.Update(adminUser);
                }
            }
        }
    }
}