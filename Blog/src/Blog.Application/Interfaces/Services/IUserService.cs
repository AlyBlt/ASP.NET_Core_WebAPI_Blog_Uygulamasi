using Blog.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace Blog.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserReadDto> AuthenticateUserAsync(string username, string password);
        Task<UserReadDto> RegisterUserAsync(RegisterDto dto);
        Task<UserReadDto> UpdateUserRoleAsync(int userId, string newRole);
        Task<UserReadDto> GetCurrentUserAsync(HttpContext httpContext);
    }
}
