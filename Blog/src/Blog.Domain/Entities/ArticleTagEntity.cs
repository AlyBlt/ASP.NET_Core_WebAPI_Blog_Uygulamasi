namespace Blog.Domain.Entities
{
    public class ArticleTagEntity
    {
        public int ArticleId { get; set; }
        public ArticleEntity Article { get; set; }

        public int TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}