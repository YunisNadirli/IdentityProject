using IdentityApi.Models;
using System.Reflection.Metadata;

namespace IdentityApi.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetUsers();
        Task<UserResponseDto> GetUser(string username);
        Task<bool> AddUser(UserRequestDto userDto);
        Task<bool> UpdateUser(string Id,UserRequestDto userDto);
        Task<bool> DeleteUser(string Id);
    }
}
