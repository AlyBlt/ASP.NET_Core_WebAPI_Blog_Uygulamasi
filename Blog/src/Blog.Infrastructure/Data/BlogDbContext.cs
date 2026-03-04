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
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User-Article ilişkisi
            modelBuilder.Entity<ArticleEntity>()
                .HasOne(a => a.User)
                .WithMany(u => u.Articles) // 1 Author → N Article
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Author silinirse makaleleri de silinir

            // Property ayarları
            modelBuilder.Entity<ArticleEntity>()
                .Property(a => a.Title)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<ArticleEntity>()
                .Property(a => a.Content)
                .IsRequired();

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
