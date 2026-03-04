using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Blog.Infrastructure.Data
{
    // EF Core CLI için gerekli factory
    public class BlogDbContextFactory : IDesignTimeDbContextFactory<BlogDbContext>
    {
        public BlogDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BlogDbContext>();

            // Buraya connection string'i hardcode veya environment variable ile verebilirsin
            optionsBuilder.UseSqlServer("Server=.;Database=BlogDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

            return new BlogDbContext(optionsBuilder.Options);
        }
    }
}