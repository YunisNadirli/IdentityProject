
using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager; 
        public UserRoleService(
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<bool> AddUserToRole(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;
            if (await _userManager.IsInRoleAsync(user, roleName)) return false;
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRole(string username, string roleName)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return false;
            if (!await _userManager.IsInRoleAsync(user, roleName)) return false;
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }
        public async Task<Dictionary<string, List<string>>> GetAllUsersWithRoles()
        {
            Dictionary<string, List<string>> userRoles = new();
            List<AppUser> users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                List<string> roles = (await _userManager.GetRolesAsync(user)).ToList();
                userRoles.Add(user.UserName, roles);
            }
            return userRoles;
        }
    }
}
