using AutoMapper;
using Blog.Application.DTOs.Tag;
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Interfaces.Services;
using Blog.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Blog.Application.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TagService> _logger;

        public TagService(ITagRepository repository, IMapper mapper, ILogger<TagService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<TagReadDto>> GetAllAsync()
        {
            _logger.LogInformation("Tüm tagler getiriliyor.");
            var tags = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TagReadDto>>(tags);
        }

        public async Task<TagReadDto?> GetByIdAsync(int id)
        {
            var tag = await _repository.GetByIdAsync(id);
            if (tag == null)
            {
                _logger.LogWarning("Id'si {Id} olan tag bulunamadı.", id);
                return null;
            }
            return _mapper.Map<TagReadDto>(tag);
        }

        public async Task<TagReadDto> CreateAsync(TagCreateDto dto)
        {
            var tag = _mapper.Map<TagEntity>(dto);
            var created = await _repository.CreateAsync(tag);
            return _mapper.Map<TagReadDto>(created);
        }

        public async Task<TagReadDto?> UpdateAsync(int id, TagUpdateDto dto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return null;

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);

            return _mapper.Map<TagReadDto>(existing);
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