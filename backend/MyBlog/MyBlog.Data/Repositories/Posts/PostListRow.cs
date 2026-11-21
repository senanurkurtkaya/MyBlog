using MyBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Posts
{
    public class PostListRow
    {
        public Guid Id { get; set; }
        public string Title { get; set; }=string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public PostStatus Status { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }

        public string ContentMarkdown { get; set; } = string.Empty;
        public List<PostCategoryRow> Categories { get; set; } = new();
    }
}
