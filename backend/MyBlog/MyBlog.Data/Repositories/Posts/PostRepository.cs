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
        private readonly AddMyBlogData _dbContext;

        public PostRepository(AddMyBlogData myBlogDbContext)
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

        public Task<List<Post>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts.AsNoTracking().ToListAsync(cancellationToken);
            
        }

        public async Task<Post?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<Post>> GetPublishedAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Posts.Where(x => x.Status == Domain.Enums.PostStatus.Published);
            return await query.ToListAsync(cancellationToken);
        }

        public Task<Post?> GetPublishedBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
        return    _dbContext.Posts.FirstOrDefaultAsync(x => x.Slug == slug && x.Status ==Domain.Enums.PostStatus.Published,cancellationToken);
            
        }
    }
}
