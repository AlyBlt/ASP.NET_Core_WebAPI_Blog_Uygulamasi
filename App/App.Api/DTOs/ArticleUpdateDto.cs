using System.ComponentModel.DataAnnotations;

namespace App.Api.DTOs
{
    public class ArticleUpdateDto
    {
        [Required]
        [MinLength(1, ErrorMessage = "Title cannot be empty.")]
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
