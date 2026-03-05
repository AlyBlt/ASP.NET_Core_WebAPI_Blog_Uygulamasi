using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = default!;
        public DateTime CreatedAt { get; set; }=DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }=DateTime.UtcNow;

        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = default!;
    }
}
