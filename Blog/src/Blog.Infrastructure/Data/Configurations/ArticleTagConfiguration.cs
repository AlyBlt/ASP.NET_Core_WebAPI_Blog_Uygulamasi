using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Configurations
{
    public class ArticleTagConfiguration : IEntityTypeConfiguration<ArticleTagEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleTagEntity> builder)
        {
            builder.HasKey(x => new { x.ArticleId, x.TagId });

            builder.HasOne(x => x.Article)
                .WithMany(x => x.ArticleTags)
                .HasForeignKey(x => x.ArticleId)
                .OnDelete(DeleteBehavior.Cascade); // Makale silinirse bağlantı silinsin

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.ArticleTags)
                .HasForeignKey(x => x.TagId)
                .OnDelete(DeleteBehavior.Cascade); // Tag silinirse bağlantı silinsin
        }
    }
}