using IdentityApi.Models;
using IdentityApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "OnlyAdmin")]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var userRoles = await _userRoleService.GetAllUsersWithRoles();
            return userRoles == null ? NotFound() : Ok(userRoles);
        }
        [HttpPost]
        public async Task<IActionResult> Post(UserRoleDto dto)
        {
            var result = await _userRoleService.AddUserToRole(dto.Username, 
                dto.Role);
            return result ? NoContent() : BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(UserRoleDto dto)
        {
            var result = await _userRoleService.RemoveUserFromRole(dto.Username, dto.Role);
            return result ? NoContent() : BadRequest(); 
        }
    }
}
