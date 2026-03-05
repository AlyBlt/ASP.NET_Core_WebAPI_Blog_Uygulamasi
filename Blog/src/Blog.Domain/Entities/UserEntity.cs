using Blog.Domain.Enums;

namespace Blog.Domain.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;

        public string PasswordHash { get; set; } = default!;

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<ArticleEntity> Articles { get; set; } = new List<ArticleEntity>();
        public ICollection<CommentEntity> Comments { get; set; } = new List<CommentEntity>();

        public UserEntity() { }

        public UserEntity(string username, string passwordHash, string email, UserRole role)
        {
            UserName = username;
            PasswordHash = passwordHash;
            Email = email;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }
    }
}