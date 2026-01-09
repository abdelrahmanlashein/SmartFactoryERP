using Microsoft.AspNetCore.Identity;
using SmartFactoryERP.Domain.Common;

namespace SmartFactoryERP.Persistence.Seeds
{
    /// <summary>
    /// Seed roles into the database
    /// </summary>
    public static class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}