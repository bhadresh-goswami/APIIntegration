using API.Core;
using API.Infrastructure.Interface;
using Microsoft.AspNetCore.Identity;

namespace API.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserRepository(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AppUser> GetUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<AppUser> GetUserByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<AppUser> CreateUser(AppUser user, string password)
        {
            AdminSeeder admin = new AdminSeeder(_userManager, _roleManager);
            await admin.SeedAsync();


            string userRoleName = "User";
            if (!await _roleManager.RoleExistsAsync(userRoleName))
            {
                var adminRole = new AppRole { Name = userRoleName };
                await _roleManager.CreateAsync(adminRole);
            }

            await _userManager.CreateAsync(user, password);


            // Assign admin role to the admin user
            if (!await _userManager.IsInRoleAsync(user, userRoleName))
            {
                await _userManager.AddToRoleAsync(user, userRoleName);
            }
            return user;
        }
    }
}
