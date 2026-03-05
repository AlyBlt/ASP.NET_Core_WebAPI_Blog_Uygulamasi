using Blog.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Blog.Application.DTOs
{
    public class ArticleUpdateDto
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ArticleStatus Status { get; set; }
    }
}
