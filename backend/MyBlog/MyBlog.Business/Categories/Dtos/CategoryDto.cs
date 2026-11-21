using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Business.Categories.Dtos;

namespace MyBlog.Business.Categories.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();
    }
}
