using API.Core;
using Microsoft.AspNetCore.Identity;


namespace API.Infrastructure.Repository
{
    public class AdminSeeder
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminSeeder(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Seed the admin role
            string adminRoleName = "Admin";
            if (!await _roleManager.RoleExistsAsync(adminRoleName))
            {
                var adminRole = new AppRole { Name = adminRoleName };
                await _roleManager.CreateAsync(adminRole);
            }

            // Seed the admin user
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser { UserName = adminEmail, Email = adminEmail };
                await _userManager.CreateAsync(adminUser, adminPassword);
            }

            // Assign admin role to the admin user
            if (!await _userManager.IsInRoleAsync(adminUser, adminRoleName))
            {
                await _userManager.AddToRoleAsync(adminUser, adminRoleName);
            }
        }
    }
}