using App.Api.Data;
using App.Api.Models;
using App.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Repositories.Implementations
{
    public class ArticleRepository : IArticleRepository
    {
        //private static List<Article> _articles = new List<Article>
        //{
        //    new Article { Id = 1, Title = "ASP.NET Core ile Web API Geliştirme", Content = "Bu makalede ASP.NET Core ile Web API geliştirmenin temellerini öğreneceksiniz.", CreatedAt = DateTime.UtcNow },
        //    new Article { Id = 2, Title = "Postman Kullanımı", Content = "Postman ile API testlerini nasıl yapabileceğinizi öğreneceksiniz.", CreatedAt = DateTime.UtcNow }
        //};

        private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }

        private static int _nextId = 3;

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            
            return await _context.Articles.FindAsync(id);
        }

        public async Task<Article> CreateAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}