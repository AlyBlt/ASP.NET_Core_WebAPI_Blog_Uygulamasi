using App.Api.DTOs;
using App.Api.Models;
using App.Api.Repositories.Interfaces;
using App.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace App.Api.Services.Implementations
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repository;
        private readonly ILogger<ArticleService> _logger;
        private readonly IMapper _mapper;


        public ArticleService(IArticleRepository repository, ILogger<ArticleService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ArticleReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Tüm makaleler isteniyor.");
            var articles = await _repository.GetAllAsync();

            _logger.LogInformation("{Count} makale bulundu.", articles.Count());

            // AutoMapper ile dönüşüm
            return _mapper.Map<IEnumerable<ArticleReadDto>>(articles);
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
            // AutoMapper ile dönüşüm
            return _mapper.Map<ArticleReadDto>(article);
        }

        public async Task<ArticleReadDto> CreateAsync(ArticleCreateDto dto)
        {
            _logger.LogInformation("Yeni makale oluşturuluyor: {@Dto}", dto);

            // AutoMapper kullanarak ArticleCreateDto'dan Article'a dönüşüm
            var article = _mapper.Map<Article>(dto);
            article.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(article);
            _logger.LogInformation("Makale oluşturuldu. Id: {Id}", created.Id);

            // AutoMapper ile dönüşüm
            return _mapper.Map<ArticleReadDto>(created);
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

            // AutoMapper ile DTO'dan Entity'ye dönüşüm/güncelleme
            _mapper.Map(dto, existing);
            // Makale güncellenmeden önce UpdatedAt alanını UTC ile güncelle
            existing.UpdatedAt = DateTime.UtcNow;

            // Veritabanındaki güncelleme işlemini Repository yapacak
            await _repository.UpdateAsync(id,existing);

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