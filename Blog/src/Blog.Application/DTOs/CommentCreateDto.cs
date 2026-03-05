namespace Blog.Application.DTOs
{
    public class CommentCreateDto
    {
        public string Content { get; set; } = default!;
        public int ArticleId { get; set; }
    }
}