using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
       private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ArticleEntity>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<ArticleEntity> GetByIdAsync(int id)
        {
            
            return await _context.Articles.FindAsync(id);
        }

        public async Task<ArticleEntity> CreateAsync(ArticleEntity article)
        {
           
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }
                                             
        public async Task UpdateAsync(ArticleEntity article)
        {
            var existing = await _context.Articles.FindAsync(article.Id);
            if (existing == null)
            {
                throw new ArgumentException("Makale bulunamadı.", nameof(article.Id));
            }

            // Mevcut article ile gelen article'ı güncelle
            existing.Title = article.Title;
            existing.Content = article.Content;
            existing.UpdatedAt = DateTime.UtcNow;


            _context.Articles.Update(existing);  // Update işlemi yapılacak nesne
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ArticleEntity article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}