namespace Blog.Domain.Entities
{
    public class CategoryEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<ArticleEntity> Articles { get; set; } = new List<ArticleEntity>();
    }
}