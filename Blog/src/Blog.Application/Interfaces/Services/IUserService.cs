using Blog.Domain.Entities;

namespace Blog.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserEntity> AuthenticateUserAsync(string username, string password);
        Task RegisterUserAsync(string username, string password, string email);
    }
}
