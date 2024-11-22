using System.Drawing;

namespace IdentityApi.Services
{
    public interface IUserRoleService
    {
        Task<bool> AddUserToRole(string username, string roleName);
        Task<bool> RemoveUserFromRole(string username, string roleName);
        Task<Dictionary<string, List<string>>> GetAllUsersWithRoles();

    }
}
