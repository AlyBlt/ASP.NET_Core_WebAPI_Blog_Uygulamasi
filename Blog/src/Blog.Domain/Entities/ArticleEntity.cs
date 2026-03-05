using Blog.Domain.Enums;

namespace Blog.Domain.Entities
    {
    public class ArticleEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; } // seo url
        public int ViewCount { get; set; }
        public ArticleStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Author
        public int AuthorId { get; set; }
        public UserEntity Author { get; set; }

        // Category
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }


        // Navigation
        public ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public ICollection<ArticleTagEntity> ArticleTags { get; set; } = new List<ArticleTagEntity>();
    }
}
