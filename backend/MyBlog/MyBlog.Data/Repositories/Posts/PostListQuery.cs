using MyBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Posts
{
    public class PostListQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PostStatus? Status { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Q { get; set; }
        public string? CategorySlug { get; set; }
    }
}
