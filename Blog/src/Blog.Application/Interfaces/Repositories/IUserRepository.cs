
using Blog.Domain.Entities;

namespace Blog.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> GetByUsernameAsync(string username);
        Task AddAsync(UserEntity user);
        Task<UserEntity> GetByIdAsync(int id);
        Task UpdateAsync(UserEntity user);
    }
}
