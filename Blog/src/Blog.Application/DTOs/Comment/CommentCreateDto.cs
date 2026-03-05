namespace Blog.Application.DTOs.Comment
{
    public class CommentCreateDto
    {
        public string Content { get; set; } = default!;
        public int ArticleId { get; set; }
    }
}