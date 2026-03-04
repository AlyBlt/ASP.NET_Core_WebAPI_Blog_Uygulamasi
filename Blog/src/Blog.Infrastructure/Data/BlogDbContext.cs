using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data

{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<ArticleEntity> Articles { get; set; }

        // Users DbSet (Yeni ekledik)
        public DbSet<UserEntity> Users { get; set; }
    }
}
