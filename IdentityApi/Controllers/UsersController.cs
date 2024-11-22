using IdentityApi.Models;
using IdentityApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy ="AdminOnly")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetUsers();    
            return users != null ? Ok(users) : NotFound();
        }
        [HttpGet("byId")]
        public async Task<IActionResult> Get(string username)
        {
            var result = await _userService.GetUser(username);
            return result != null ? Ok(result) : NotFound();
        }
        [HttpPost("add")]
        public async Task<IActionResult> Post(UserRequestDto dto)
        {
            var result = await _userService.AddUser(dto);
            return result == true ? NoContent() : BadRequest();
        }
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string Id)
        {
            var result = await _userService.DeleteUser(Id);
            return result ? NoContent() : BadRequest();
        }
        [HttpPut("edit")]
        public async Task<IActionResult> Update(string Id, UserRequestDto dto)
        {
            var result = await _userService.UpdateUser(Id, dto);
            return result ? NoContent() : BadRequest();
        }

    }
}
