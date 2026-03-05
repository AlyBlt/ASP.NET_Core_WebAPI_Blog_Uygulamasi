using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IEnumerable<TagEntity>> GetAllAsync();
        Task<TagEntity?> GetByIdAsync(int id);
        Task<TagEntity> CreateAsync(TagEntity tag);
        Task UpdateAsync(TagEntity tag);
        Task DeleteAsync(TagEntity tag);
    }
}
