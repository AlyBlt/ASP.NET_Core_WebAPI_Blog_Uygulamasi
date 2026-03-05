namespace Blog.Domain.Entities
{
    public class TagEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<ArticleTagEntity> ArticleTags { get; set; } = new List<ArticleTagEntity>();
    }
}