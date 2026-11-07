using MyBlog.Data.Contexts;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Posts
{
    public interface IPostRepository
    {
        
        public void Add(Post post);
        Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default);
    }
}
