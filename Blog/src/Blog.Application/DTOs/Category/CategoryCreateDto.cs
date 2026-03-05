using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs.Category
{
    public class CategoryCreateDto
    {
        public string Name { get; set; } = default!;
        public string Slug { get; set; } = default!; 
    }
}
