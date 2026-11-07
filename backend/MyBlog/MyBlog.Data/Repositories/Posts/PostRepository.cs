using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Contexts;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Data.Repositories.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly MyBlogDbContext _dbContext;

        public PostRepository(MyBlogDbContext myBlogDbContext)
        {

            _dbContext = myBlogDbContext;
        }
        public void Add(Post post)
        {
            _dbContext.Posts.Add(post);
        }

        public Task<bool> ExistsBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
           return _dbContext.Posts.AnyAsync(x => x.Slug == slug , cancellationToken);
        }
    }
}
