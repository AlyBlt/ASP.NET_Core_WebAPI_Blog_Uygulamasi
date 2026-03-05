using Blog.Application.DTOs.Auth;
using Blog.Application.DTOs.User;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserReadDto?> AuthenticateUserAsync(string username, string password);
        Task<UserReadDto> RegisterUserAsync(RegisterDto dto);
        Task<UserReadDto?> UpdateUserRoleAsync(int userId, UserRole newRole);
        Task<UserReadDto?> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal);
    }
}
