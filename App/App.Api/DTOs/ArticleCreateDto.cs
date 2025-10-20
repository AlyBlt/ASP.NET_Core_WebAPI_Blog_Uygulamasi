using System.ComponentModel.DataAnnotations;

namespace App.Api.DTOs
{
    public class ArticleCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Başlık 5-100 karakter arasında olmalıdır..")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "İçerik 10-500 karakter arasında olmalıdır..")]
        public string Content { get; set; }
    }
}
