using Blog.Application.DTOs;


namespace Blog.Application.Interfaces.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentReadDto>> GetAllAsync();
        Task<CommentReadDto?> GetByIdAsync(int id);
        Task<IEnumerable<CommentReadDto>> GetByArticleIdAsync(int articleId);
        Task<CommentReadDto> CreateAsync(CommentCreateDto dto, int userId);
        Task<CommentReadDto?> UpdateAsync(int id, CommentUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
