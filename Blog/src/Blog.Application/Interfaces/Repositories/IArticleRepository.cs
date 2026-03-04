
using Blog.Domain.Entities;

namespace Blog.Application.Interfaces.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<ArticleEntity>> GetAllAsync();
        Task<ArticleEntity> GetByIdAsync(int id);
        Task<ArticleEntity> CreateAsync(ArticleEntity article);
        Task UpdateAsync(ArticleEntity article);
        Task DeleteAsync(ArticleEntity article);
    }
}
