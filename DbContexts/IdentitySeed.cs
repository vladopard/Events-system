using Events_system.Entities;
using Microsoft.AspNetCore.Identity;

namespace Events_system.DbContexts
{
    public static class IdentitySeed
    {
        public static async Task EnsureSeedDataAsync(this IServiceProvider services)
        {
            var userMgr = services.GetRequiredService<UserManager<User>>();
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

            // 1) create roles
            var roles = new[] { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole(role));
            }

            // 2) create super admin
            var adminEmail = "admin@example.com";
            var admin = await userMgr.FindByEmailAsync(adminEmail);

            if(admin == null)
            {
                admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    DateJoined = DateTime.UtcNow
                };
                var res = await userMgr.CreateAsync(admin, "P@sswOrd1");
                if(!res.Succeeded)
                    throw new Exception($"Failed to create admin user: " +
                        $"{string.Join(", ", res.Errors.Select(e => e.Description))}");
            }
            // 3) assign Admin role
            if (!await userMgr.IsInRoleAsync(admin, "Admin"))
                await userMgr.AddToRoleAsync(admin, "Admin");
        }
    }
}
