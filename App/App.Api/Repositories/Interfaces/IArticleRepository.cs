using Microsoft.Extensions.Hosting;
using App.Api.Models;
using AutoMapper;

namespace App.Api.Repositories.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int id);
        Task<Article> CreateAsync(Article article);
        Task UpdateAsync(int id, Article article);
        Task DeleteAsync(Article article);
    }
}
