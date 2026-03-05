using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDbContext _context;

        public CategoryRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryEntity>> GetAllAsync()
        {
            return await _context.Categories
                                 .Include(c => c.Articles) // İsteğe bağlı, makaleleri dahil edebiliriz
                                 .ToListAsync();
        }

        public async Task<CategoryEntity?> GetByIdAsync(int id)
        {
            return await _context.Categories
                                 .Include(c => c.Articles)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CategoryEntity> CreateAsync(CategoryEntity category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task UpdateAsync(CategoryEntity category)
        {
            var existing = await _context.Categories.FindAsync(category.Id);
            if (existing == null)
                throw new ArgumentException("Kategori bulunamadı.", nameof(category.Id));

            existing.Name = category.Name;
            existing.Slug = category.Slug;

            _context.Categories.Update(existing);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CategoryEntity category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}