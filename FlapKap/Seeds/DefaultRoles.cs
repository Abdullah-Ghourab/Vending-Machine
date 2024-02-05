using FlapKap.Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace FlapKap.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Buyer));
                await roleManager.CreateAsync(new IdentityRole(AppRoles.Seller));
            }
        }
    }
}
