using App.Api.DTOs;

namespace App.Api.Services.Interfaces
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleReadDto>> GetAllAsync();
        Task<ArticleReadDto> GetByIdAsync(int id);
        Task<ArticleReadDto> CreateAsync(ArticleCreateDto dto);
        Task UpdateAsync(int id, ArticleUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
