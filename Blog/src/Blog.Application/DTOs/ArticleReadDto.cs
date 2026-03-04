namespace Blog.Application.DTOs
{
    public class ArticleReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Makaleyi kimin oluşturduğunu bilmek için
        public int UserId { get; set; }
        public string Username { get; set; } // opsiyonel, sadece göstermek için

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
