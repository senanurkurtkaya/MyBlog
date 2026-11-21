using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Comments.Dtos
{
    public class CreateCommentDto
    {
        public string AuthorName { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }

    }
}
