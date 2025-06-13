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
                    Id = "admin-id-001",
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

            // 4) create regular customer
            var customerEmail = "user@example.com";
            var customer = await userMgr.FindByEmailAsync(customerEmail);

            if (customer == null)
            {
                customer = new User
                {
                    Id = "seed-user-id", // фиксирани ID ако користиш у .HasData()
                    UserName = customerEmail,
                    Email = customerEmail,
                    FirstName = "Regular",
                    LastName = "User",
                    DateJoined = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var res = await userMgr.CreateAsync(customer, "User@123");
                if (!res.Succeeded)
                    throw new Exception($"Failed to create test user: " +
                        $"{string.Join(", ", res.Errors.Select(e => e.Description))}");
            }

            // 5) assign Customer role
            if (!await userMgr.IsInRoleAsync(customer, "Customer"))
                await userMgr.AddToRoleAsync(customer, "Customer");
        }
    }
}
