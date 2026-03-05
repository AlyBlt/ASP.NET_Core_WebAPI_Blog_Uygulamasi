using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(BlogDbContext context)
        {
            if (await context.Users.AnyAsync() ||
                 await context.Categories.AnyAsync() ||
                 await context.Articles.AnyAsync() ||
                 await context.Tags.AnyAsync() ||
                 await context.Comments.AnyAsync())
            {
                return;
            }

            var passwordHasher = new PasswordHasher<UserEntity>();

            // ===== Users =====
            var admin = new UserEntity("admin",
                passwordHasher.HashPassword(null, "Admin123!"),
                "admin@blog.com",
                UserRole.Admin);

            var author1 = new UserEntity("author",
                passwordHasher.HashPassword(null, "Author123!"),
                "author@blog.com",
                UserRole.Author);

            var author2 = new UserEntity("authorB",
                passwordHasher.HashPassword(null, "AuthorB123!"),
                "authorb@blog.com",
                UserRole.Author);

            var user = new UserEntity("user",
                passwordHasher.HashPassword(null, "User123!"),
                "user@blog.com",
                UserRole.User);

            await context.Users.AddRangeAsync(admin, author1, author2, user);

            // ===== Categories =====
            var category1 = new CategoryEntity
            {
                Name = "Software Architecture",
                Slug = "software-architecture"
            };

            var category2 = new CategoryEntity
            {
                Name = "Security",
                Slug = "security"
            };

            await context.Categories.AddRangeAsync(category1, category2);

            // ===== Tags =====
            var tag1 = new TagEntity { Name = "Architecture" };
            var tag2 = new TagEntity { Name = "Security" };
            var tag3 = new TagEntity { Name = "JWT" };
            await context.Tags.AddRangeAsync(tag1, tag2, tag3);


            // ===== Articles =====
            var articles = new List<ArticleEntity>
            {
                new ArticleEntity
                {
                    Title = "Clean Architecture Nedir?",
                    Content = "Clean Architecture katmanlı bir yazılım mimarisidir...",
                    Slug = "clean-architecture-nedir",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Author = author1,
                    Category = category1,
                    Status = ArticleStatus.Published
                },
                new ArticleEntity
                {
                    Title = "JWT Authentication",
                    Content = "JWT, stateless authentication sağlar ve token bazlıdır...",
                    Slug = "jwt-authentication",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Author = author2,
                    Category = category2,
                    Status = ArticleStatus.Published
                }
            };
            await context.Articles.AddRangeAsync(articles);

            // ===== ArticleTags =====
            var articleTags = new List<ArticleTagEntity>
            {
                new ArticleTagEntity { Article = articles[0], Tag = tag1 },
                new ArticleTagEntity { Article = articles[1], Tag = tag2 },
                new ArticleTagEntity { Article = articles[1], Tag = tag3 }
            };
            await context.ArticleTags.AddRangeAsync(articleTags);

            // ===== Comments =====
            var comments = new List<CommentEntity>
            {
                new CommentEntity
                {
                    Content = "Harika makale!",
                    CreatedAt = DateTime.UtcNow,
                    Article = articles[0],
                    User = user
                },
                new CommentEntity
                {
                    Content = "JWT çok faydalı bir konu.",
                    CreatedAt = DateTime.UtcNow,
                    Article = articles[1],
                    User = user
                },
                new CommentEntity
                {
                    Content = "Ben de bunu okudum, çok iyi yazılmış.",
                    CreatedAt = DateTime.UtcNow,
                    Article = articles[0],
                    User = author2
                }
            };
            await context.Comments.AddRangeAsync(comments);

            // ===== Save Changes =====
            await context.SaveChangesAsync();
        }
    }
}