using App.Api.Data;
using App.Api.Models;
using App.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace App.Api.Repositories.Implementations
{
    public class ArticleRepository : IArticleRepository
    {
       private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }

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
                                             
        public async Task UpdateAsync(int id, Article article)
        {
            var existing = await _context.Articles.FindAsync(id);
            if (existing == null)
            {
                throw new ArgumentException("Makale bulunamadı.", nameof(id));
            }

            // Mevcut article ile gelen article'ı güncelle
            existing.Title = article.Title;
            existing.Content = article.Content;
            

            _context.Articles.Update(existing);  // Update işlemi yapılacak nesne
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}