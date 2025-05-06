
using Catering.Models;
using Microsoft.AspNetCore.Identity;
namespace Catering.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedRolesAsync(IServiceProvider services)
        {
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new[] { "Admin", "Customer", "Caterer" };

            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@catering.com", adminPwd = "P@ssword1";
            if (await userMgr.FindByEmailAsync(adminEmail) is null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userMgr.CreateAsync(admin, adminPwd);
                await userMgr.AddToRoleAsync(admin, "Admin");
            }
        }

    }
}
