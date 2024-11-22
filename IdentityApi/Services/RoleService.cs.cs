using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Services
{
    
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager; 
        }

        public async Task<bool> AddRole(RoleDto roleDto)
        {
            var role = DtoToEntity(roleDto);
            role.Id = Guid.NewGuid().ToString();
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null) return false;
            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded;
        }

        public async Task<RoleDto> GetRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            return EntityToDto(role);
        }

        public List<RoleDto> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            var roleDtos = new List<RoleDto>();
            foreach (var role in roles)
            {
                roleDtos.Add(EntityToDto(role));
            }
            return roleDtos;
        }

        public async Task<bool> UpdateRole(string Id, RoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            role.Name = roleDto.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;    
        }


        RoleDto EntityToDto(AppRole role)
        {
            RoleDto dto = new RoleDto()
            {
                Id = role.Id,
                RoleName = role.Name
            };
            return dto;
        }

        AppRole DtoToEntity(RoleDto dto)
        {
            AppRole role = new AppRole()
            {
                Id = dto.Id,
                Name = dto.RoleName
            };
            return role;
        }
    }
}
