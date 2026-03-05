using Blog.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.Repositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<CommentEntity>> GetAllAsync();
        Task<IEnumerable<CommentEntity>> GetByArticleIdAsync(int articleId);
        Task<CommentEntity?> GetByIdAsync(int id);
        Task<CommentEntity> CreateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity comment);
        Task DeleteAsync(CommentEntity comment);
    }
}