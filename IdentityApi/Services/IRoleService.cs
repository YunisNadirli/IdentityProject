using IdentityApi.Models;

namespace IdentityApi.Services
{
    public interface IRoleService
    {
        List<RoleDto> GetRoles();
        Task<RoleDto> GetRole(string roleName);
        Task<bool> AddRole(RoleDto roleDto);
        Task<bool> UpdateRole(string Id, RoleDto roleDto);
        Task<bool> DeleteRole(string Id);
    }
}
