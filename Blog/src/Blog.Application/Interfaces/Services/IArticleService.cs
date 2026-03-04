using Blog.Application.DTOs;

namespace Blog.Application.Interfaces.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<ArticleReadDto>> GetAllAsync();
        Task<ArticleReadDto> GetByIdAsync(int id);
        Task<ArticleReadDto> CreateAsync(ArticleCreateDto dto, int authorId);
        Task<ArticleReadDto> UpdateAsync(int id, ArticleUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
