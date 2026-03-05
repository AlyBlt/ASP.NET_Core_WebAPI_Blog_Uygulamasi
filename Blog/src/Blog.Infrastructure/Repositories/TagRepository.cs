using Blog.Application.Interfaces.Repositories;
using Blog.Domain.Entities;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BlogDbContext _context;

        public TagRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TagEntity>> GetAllAsync()
        {
            return await _context.Tags
                                 .Include(t => t.ArticleTags) // opsiyonel: makalelerle ilişkili görmek için
                                 .ToListAsync();
        }

        public async Task<TagEntity?> GetByIdAsync(int id)
        {
            return await _context.Tags
                                 .Include(t => t.ArticleTags)
                                 .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<TagEntity> CreateAsync(TagEntity tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task UpdateAsync(TagEntity tag)
        {
            var existing = await _context.Tags.FindAsync(tag.Id);
            if (existing == null)
                throw new ArgumentException("Tag bulunamadı.", nameof(tag.Id));

            existing.Name = tag.Name;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TagEntity tag)
        {
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
        }
    }
}