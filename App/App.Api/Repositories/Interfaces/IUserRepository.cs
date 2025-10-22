using App.Api.Models;

namespace App.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
        Task<User> GetByIdAsync(int id);
        Task UpdateAsync(User user);
    }
}
