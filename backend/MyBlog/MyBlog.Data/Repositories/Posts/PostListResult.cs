using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Posts
{
    public class PostListResult
    {
        public List<PostListRow> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
