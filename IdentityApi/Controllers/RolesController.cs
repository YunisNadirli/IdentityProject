using IdentityApi.Models;
using IdentityApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "OnlyAdmin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet("all")]
        public IActionResult Get()
        {
            var roles = _roleService.GetRoles();
            return roles != null ? Ok(roles) : NotFound();
        }
        [HttpGet("username:string")]
        public async Task<IActionResult> Get(string roleName)
        {
            var result = await _roleService.GetRole(roleName);
            return result != null ? Ok(result) : NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Post(RoleDto dto)
        {
            var result = await _roleService.AddRole(dto);
            return result ? NoContent() : BadRequest();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _roleService.DeleteRole(Id);
            return result ? NoContent() : BadRequest();
        }
        [HttpPut("Id:string")]
        public async Task<IActionResult> Put(string Id, RoleDto dto)
        {
            var result = await _roleService.UpdateRole(Id, dto);
            return result ? NoContent() : BadRequest();
        }
    }
}
