using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Posts.Dtos
{
    public class CreatePostDto
    {
        public string Title { get; set; }=string.Empty;
        public string Slug { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public string? Summary { get; set; }
        public string? ContentMarkdown { get; set; }
    }
}
