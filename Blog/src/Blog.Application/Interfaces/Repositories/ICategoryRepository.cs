using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<CategoryEntity>> GetAllAsync();
        Task<CategoryEntity?> GetByIdAsync(int id);
        Task<CategoryEntity> CreateAsync(CategoryEntity category);
        Task UpdateAsync(CategoryEntity category);
        Task DeleteAsync(CategoryEntity category);
    }
}
