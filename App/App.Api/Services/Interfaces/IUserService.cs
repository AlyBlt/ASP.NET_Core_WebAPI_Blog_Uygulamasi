using App.Api.Models;

namespace App.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateUserAsync(string username, string password);
        Task RegisterUserAsync(string username, string password, string email);
    }
}
