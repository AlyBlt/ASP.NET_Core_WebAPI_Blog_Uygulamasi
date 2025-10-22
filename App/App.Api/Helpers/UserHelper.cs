using App.Api.Models;
using App.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace App.Api.Helpers
{
    public static class UserHelper
    {
        public static async Task<User?> GetCurrentUserAsync(HttpContext httpContext, IUserRepository userRepository)
        {
            var username = httpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            return await userRepository.GetByUsernameAsync(username);
        }
    }
}