using Blog.Domain.Enums;
using System.Text.Json.Serialization;

namespace Blog.Application.DTOs.Article
{
    public class ArticleReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Slug { get; set; } = default!;
        public string Content { get; set; } = default!;
        public int ViewCount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ArticleStatus Status { get; set; }
        // Author
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = default!;
        // Category
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
