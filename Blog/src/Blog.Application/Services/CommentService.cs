using AutoMapper;
using Blog.Application.DTOs;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Interfaces.Services;
using Blog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Blog.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(ICommentRepository repository, IMapper mapper, ILogger<CommentService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CommentReadDto>> GetAllAsync()
        {
            var comments = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentReadDto>>(comments);
        }

        public async Task<CommentReadDto?> GetByIdAsync(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            return comment == null ? null : _mapper.Map<CommentReadDto>(comment);
        }

        public async Task<IEnumerable<CommentReadDto>> GetByArticleIdAsync(int articleId)
        {
            var comments = await _repository.GetByArticleIdAsync(articleId);
            return _mapper.Map<IEnumerable<CommentReadDto>>(comments);
        }

        public async Task<CommentReadDto> CreateAsync(CommentCreateDto dto, int userId)
        {
            var comment = _mapper.Map<CommentEntity>(dto);
            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            var created = await _repository.CreateAsync(comment);
            return _mapper.Map<CommentReadDto>(created);
        }

        public async Task<CommentReadDto?> UpdateAsync(int id, CommentUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

            return _mapper.Map<CommentReadDto>(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}