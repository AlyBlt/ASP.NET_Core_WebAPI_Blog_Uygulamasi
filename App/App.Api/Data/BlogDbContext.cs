using App.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Api.Data

{
    public class BlogDbContext : DbContext
    {

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }
    }
}
