using IdentityApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Drawing.Text;

namespace IdentityApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserService(
            UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserResponseDto>> GetUsers()
        {
            var userList = _userManager.Users.ToList(); 
            var dtos = new List<UserResponseDto>(); 
            foreach (var user in userList)
            {
                var userDto = await EntityToDto(user);
                dtos.Add(userDto);
            }
            return dtos;    
        }

        public async Task<UserResponseDto> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null) return await EntityToDto(user);
            return null;
        }

        public async Task<bool> AddUser(UserRequestDto userDto)
        {
            var user = await DtoToEntity(userDto);
            if (user != null)
            {
                var result = await _userManager.CreateAsync(user);
                if (_roleManager.FindByNameAsync("user") == null)
                {
                    AppRole role = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = "user"
                    };
                    await _roleManager.CreateAsync(role);
                }
                await _userManager.AddToRoleAsync(user, "user");
                return result.Succeeded;
            }
            return false;
        }

        public async Task<bool> UpdateUser(string Id, UserRequestDto userDto)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) return false;
            user.UserName = userDto.Username;
            user.FullName = userDto.Fullname;
            user.Email = userDto.Email;
            user.PasswordHash = userDto.Password;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
        public async Task<bool> DeleteUser(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null) return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        async Task<UserResponseDto> EntityToDto(AppUser user)
        {
            UserResponseDto dto = new UserResponseDto()
            {
                Id = user.Id,
                Username = user.UserName,
                Fullname = user.FullName,
                Email = user.Email,
                Password = user.PasswordHash,
            };
            return dto;
        }

        async Task<AppUser> DtoToEntity(UserRequestDto dto)
        {
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = dto.Username,
                FullName = dto.Fullname,
                Email = dto.Email,
                PasswordHash = dto.Password
            };
            return user;
        }

        
    }
}
