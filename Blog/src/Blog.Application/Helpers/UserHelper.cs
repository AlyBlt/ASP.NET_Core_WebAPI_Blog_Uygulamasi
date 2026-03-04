using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Application.Helpers
{
    public static class UserHelper
    {
        public static async Task<UserEntity?> GetCurrentUserAsync(HttpContext httpContext, IUserRepository userRepository)
        {
            var username = httpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            return await userRepository.GetByUsernameAsync(username);
        }
    }
}