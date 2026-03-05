using Blog.Application.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.Services
{
    public interface ITagService
    {
        Task<IEnumerable<TagReadDto>> GetAllAsync();
        Task<TagReadDto?> GetByIdAsync(int id);
        Task<TagReadDto> CreateAsync(TagCreateDto dto);
        Task<TagReadDto?> UpdateAsync(int id, TagUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
