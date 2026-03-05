using Blog.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Application.DTOs.Article
{
    public class ArticleUpdateDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        public int CategoryId { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ArticleStatus Status { get; set; }
    }
}
