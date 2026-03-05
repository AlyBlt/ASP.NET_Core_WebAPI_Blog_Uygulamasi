using AutoMapper;
using Blog.Application.DTOs.Article;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Interfaces.Services;
using Blog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Blog.Application.Services
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

            return _mapper.Map<IEnumerable<ArticleReadDto>>(articles);
        }

        public async Task<ArticleReadDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale isteniyor.", id);

            var article = await _repository.GetByIdAsync(id);
            if (article == null)
            {
                _logger.LogWarning("Id'si {Id} olan makale bulunamadı.", id);
                return null;
            }

            return _mapper.Map<ArticleReadDto>(article);
        }

        public async Task<ArticleReadDto> CreateAsync(ArticleCreateDto dto, int authorId)
        {
            _logger.LogInformation("Yeni makale oluşturuluyor: {@Dto}", dto);

            // ArticleEntity oluşturuluyor
            var article = new ArticleEntity
            {
                Title = dto.Title,
                Content = dto.Content,
                AuthorId = authorId,
                CategoryId = dto.CategoryId,
                Status = dto.Status,
                Slug = GenerateSlug(dto.Title),  // Slug'ı title'dan oluşturuyoruz
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // TagIds ile ilişki kurma (ArticleTagEntity)
            foreach (var tagId in dto.TagIds)
            {
                article.ArticleTags.Add(new ArticleTagEntity
                {
                    ArticleId = article.Id,  // Makale id'si, henüz kaydedilmediği için boş
                    TagId = tagId            // İlgili etiket id'si
                });
            }


            var created = await _repository.CreateAsync(article);
            _logger.LogInformation("Makale oluşturuldu. Id: {Id}", created.Id);

            return _mapper.Map<ArticleReadDto>(created);
        }

        public async Task<ArticleReadDto?> UpdateAsync(int id, ArticleUpdateDto dto)
        {
            _logger.LogInformation("Id'si {Id} olan makale güncelleniyor.", id);

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Güncelleme başarısız. Id {Id} bulunamadı.", id);
                return null;
            }

            _mapper.Map(dto, existing);
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
            _logger.LogInformation("Makale güncellendi. Id: {Id}", id);

            return _mapper.Map<ArticleReadDto>(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale silinmek isteniyor.", id);

            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            _logger.LogInformation("Makale silindi. Id: {Id}", id);

            return true;
        }

        // Slug'ı başlıktan oluşturma fonksiyonu
        private string GenerateSlug(string title)
        {
            return title.ToLower().Trim().Replace(" ", "-"); // Basit bir slug örneği
        }
    }
}