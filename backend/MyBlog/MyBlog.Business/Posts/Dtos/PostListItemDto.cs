using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Posts.Dtos
{
    public class PostListItemDto
    {
        public string Title { get; set; }=string.Empty;
        public string Slug { get; set; }= string.Empty;
        public string? Summary { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
    }
}
