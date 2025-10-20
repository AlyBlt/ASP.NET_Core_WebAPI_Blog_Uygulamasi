using App.Api.DTOs;
using App.Api.Models;
using App.Api.Repositories.Interfaces;
using App.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace App.Api.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(IArticleRepository repository, ILogger<ArticleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<ArticleReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Tüm makaleler isteniyor.");
            var articles = await _repository.GetAllAsync();

            _logger.LogInformation("{Count} makale bulundu.", articles.Count());

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
            _logger.LogInformation("Id'si {Id} olan makale isteniyor.", id);

            var article = await _repository.GetByIdAsync(id);
            if (article == null)
            {
                _logger.LogWarning("Id'si {Id} olan makale bulunamadı.", id);
                return null;
            }

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
            _logger.LogInformation("Yeni makale oluşturuluyor: {@Dto}", dto);
            var article = new Article
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(article);
            _logger.LogInformation("Makale oluşturuldu. Id: {Id}", created.Id);

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
            _logger.LogInformation("Id'si {Id} olan makale güncelleniyor.", id);

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Güncelleme başarısız. Id {Id} bulunamadı.", id);
                return;
            }

            existing.Title = dto.Title;
            existing.Content = dto.Content;

            await _repository.UpdateAsync(existing);
            _logger.LogInformation("Makale güncellendi. Id: {Id}", id);
        }

        public async Task DeleteAsync(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale silinmek isteniyor.", id);
            var article = await _repository.GetByIdAsync(id);
            if (article == null)
            {
                _logger.LogWarning("Silme başarısız. Id {Id} bulunamadı.", id);
                return;
            }

            await _repository.DeleteAsync(article);
            _logger.LogInformation("Makale silindi. Id: {Id}", id);

        }
    }
}