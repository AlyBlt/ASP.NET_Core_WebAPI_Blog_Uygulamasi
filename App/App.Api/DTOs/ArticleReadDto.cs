namespace App.Api.DTOs
{
    public class ArticleReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        private DateTime _createdAt;
        private DateTime _updatedAt;

        public DateTime CreatedAt
        {
            get => _createdAt.ToLocalTime();
            set => _createdAt = value;
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt.ToLocalTime();
            set => _updatedAt = value;
        }

    }
}
