using Microsoft.AspNetCore.Identity;

namespace InsuranceWebApp.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            if(!await roleManager.RoleExistsAsync(UserRoles.Client))
            {
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Client));
            }
        }
    }
}
