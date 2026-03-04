using Blog.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedAsync(BlogDbContext context)
        {
            // DB doluysa tekrar seed yapma
            if (await context.Users.AnyAsync() || await context.Articles.AnyAsync())
                return;

            var passwordHasher = new PasswordHasher<UserEntity>();

            // Admin
            var admin = new UserEntity(
                "admin",
                passwordHasher.HashPassword(null, "Admin123!"),
                "admin@blog.com",
                "Admin"
            );

            // Author
            var author = new UserEntity(
                "author",
                passwordHasher.HashPassword(null, "Author123!"),
                "author@blog.com",
                "Author"
            );

            var user = new UserEntity(
               "user",
               passwordHasher.HashPassword(null, "User123!"),
               "user@blog.com",
               "User"
           );

            // Makaleler Author'a bağlı
            var articles = new List<ArticleEntity>
            {
                new ArticleEntity
                {
                    Title = "Clean Architecture Nedir?",
                    Content = "Clean Architecture katmanlı bir yazılım mimarisidir...",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    User = author
                },
                new ArticleEntity
                {
                    Title = "JWT Authentication",
                    Content = "JWT, stateless authentication sağlar ve token bazlıdır...",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    User = author
                },

            };

            await context.Users.AddRangeAsync(admin, author, user);
            await context.Articles.AddRangeAsync(articles);

            await context.SaveChangesAsync();
        }
    }
}