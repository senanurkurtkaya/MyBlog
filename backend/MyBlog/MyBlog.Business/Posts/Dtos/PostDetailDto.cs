using MyBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBlog.Business.Categories.Dtos;

namespace MyBlog.Business.Posts.Dtos
{
    public class PostDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }=string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ContentMarkdown { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public PostStatus  Status { get; set; }
        public Guid? CoverImageId { get; set; }

        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }

        public int ReadingTimeMinutes { get; set; }

        public List<CategoryDto> Categories { get; set; } = new();
    }

}
