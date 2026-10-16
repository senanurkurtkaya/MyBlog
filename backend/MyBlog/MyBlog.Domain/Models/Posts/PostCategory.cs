using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models.Posts
{
    public class PostCategory
    {
        public Guid PostId { get;private set; }
        public Guid CategoryId { get; private set; }

        public PostCategory(Guid postId,Guid categoryId)
        {
            if(postId == Guid.Empty)
            {
                throw new ArgumentException("Yazı kimliği boş olamaz.",nameof(postId));
            }
            if(categoryId == Guid.Empty)
            {
                throw new ArgumentException("Kategori kimliği boş olamaz.",nameof(categoryId));
            }
            PostId = postId;
            CategoryId = categoryId;
        }

    }
}
