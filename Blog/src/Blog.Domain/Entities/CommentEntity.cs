namespace Blog.Domain.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Article
        public int ArticleId { get; set; }
        public ArticleEntity Article { get; set; } = default!;

        // User
        public int UserId { get; set; }
        public UserEntity User { get; set; } = default!;
    }
}