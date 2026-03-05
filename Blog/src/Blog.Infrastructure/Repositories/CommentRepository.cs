using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _context;

        public CommentRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CommentEntity>> GetAllAsync()
        {
            return await _context.Comments
                                 .Include(c => c.User)
                                 .Include(c => c.Article)
                                 .ToListAsync();
        }

        public async Task<CommentEntity?> GetByIdAsync(int id)
        {
            return await _context.Comments
                                 .Include(c => c.User)
                                 .Include(c => c.Article)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<CommentEntity>> GetByArticleIdAsync(int articleId)
        {
            return await _context.Comments
                                 .Include(c => c.User)
                                 .Where(c => c.ArticleId == articleId)
                                 .ToListAsync();
        }

        public async Task<CommentEntity> CreateAsync(CommentEntity comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CommentEntity comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}