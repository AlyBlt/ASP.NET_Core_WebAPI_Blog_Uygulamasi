using App.Api.DTOs;
using App.Api.Models;
using App.Api.Repositories.Interfaces;
using App.Api.Services.Interfaces;

namespace App.Api.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;

        public ArticleService(IArticleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ArticleReadDto>> GetAllAsync()
        {
            var articles = await _repository.GetAllAsync();

            return articles.Select(a => new ArticleReadDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                CreatedAt = a.CreatedAt
            }).ToList();
        }

        public async Task<ArticleReadDto> GetByIdAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null) return null;

            return new ArticleReadDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt
            };
        }

        public async Task<ArticleReadDto> CreateAsync(ArticleCreateDto dto)
        {
            var article = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(article);

            return new ArticleReadDto
            {
                Id = created.Id,
                Title = created.Title,
                Content = created.Content,
                CreatedAt = created.CreatedAt
            };
        }

        public async Task UpdateAsync(int id, ArticleUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return;

            existing.Title = dto.Title;
            existing.Content = dto.Content;

            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _repository.GetByIdAsync(id);
            if (article == null) return;

            await _repository.DeleteAsync(article);
        }
    }
}