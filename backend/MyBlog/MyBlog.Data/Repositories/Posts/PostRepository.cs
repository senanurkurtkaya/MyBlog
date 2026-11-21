using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Contexts;
using MyBlog.Domain.Enums;
using MyBlog.Domain.Models.Posts;

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

        public void Remove(Post post)
        {
            _dbContext.Posts.Remove(post);
        }

        public Task<bool> ExistsBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts.AnyAsync(
                post => post.Slug == slug,
                cancellationToken);
        }

        public Task<List<Post>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public Task<Post?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts
                .Include(post => post.PostCategories)
                .ThenInclude(postCategory => postCategory.Category)
                .FirstOrDefaultAsync(
                    post => post.Id == id,
                    cancellationToken);
        }

        public Task<List<Post>> GetPublishedAsync(
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts
                .Where(post => post.Status == PostStatus.Published)
                .ToListAsync(cancellationToken);
        }

        public Task<Post?> GetPublishedBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Posts
                .Include(post => post.PostCategories)
                .ThenInclude(postCategory => postCategory.Category)
                .FirstOrDefaultAsync(
                    post => post.Slug == slug &&
                            post.Status == PostStatus.Published,
                    cancellationToken);
        }

        public async Task<PostListResult> GetAdminListAsync(
            PostListQuery query,
            CancellationToken cancellationToken = default)
        {
            var postsQuery = _dbContext.Posts.AsNoTracking();

            if (query.Status.HasValue)
            {
                postsQuery = postsQuery.Where(
                    post => post.Status == query.Status.Value);
            }

            if (query.CategoryId.HasValue)
            {
                postsQuery = postsQuery.Where(post =>
                    post.PostCategories.Any(postCategory =>
                        postCategory.CategoryId == query.CategoryId.Value));
            }

            if (query.FromDate.HasValue)
            {
                postsQuery = postsQuery.Where(
                    post => post.CreatedAtUtc >= query.FromDate.Value);
            }

            if (query.ToDate.HasValue)
            {
                postsQuery = postsQuery.Where(
                    post => post.CreatedAtUtc <= query.ToDate.Value);
            }

            postsQuery = ApplySearch(postsQuery, query.Q);

            var totalCount = await postsQuery.CountAsync(cancellationToken);

            postsQuery = postsQuery
                .OrderByDescending(
                    post => post.UpdatedAtUtc ?? post.CreatedAtUtc)
                .ThenByDescending(post => post.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            var items = await postsQuery
                .Select(post => new PostListRow
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    Summary = post.Summary,
                    Status = post.Status,
                    CreatedAtUtc = post.CreatedAtUtc,
                    PublishedAtUtc = post.PublishedAtUtc,
                    UpdatedAtUtc = post.UpdatedAtUtc,

                    Categories = post.PostCategories
                        .Select(postCategory => new PostCategoryRow
                        {
                            Id = postCategory.CategoryId,
                            Name = postCategory.Category.Name,
                            Color = postCategory.Category.Color,
                            Slug = postCategory.Category.Slug,
                            DisplayOrder = postCategory.Category.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return new PostListResult
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<PostListResult> GetPublicListAsync(
            PostListQuery query,
            CancellationToken cancellationToken = default)
        {
            var postsQuery = _dbContext.Posts
                .AsNoTracking()
                .Where(post => post.Status == PostStatus.Published);

            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
            {
                var categorySlug = query.CategorySlug.Trim();

                postsQuery = postsQuery.Where(post =>
                    post.PostCategories.Any(postCategory =>
                        postCategory.Category.Slug == categorySlug));
            }

            postsQuery = ApplySearch(postsQuery, query.Q);

            var totalCount = await postsQuery.CountAsync(cancellationToken);

            postsQuery = postsQuery
                .OrderByDescending(post => post.PublishedAtUtc)
                .ThenByDescending(post => post.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            var items = await postsQuery
                .Select(post => new PostListRow
                {
                    Id = post.Id,
                    Title = post.Title,
                    Slug = post.Slug,
                    Summary = post.Summary,
                    Status = post.Status,
                    PublishedAtUtc = post.PublishedAtUtc,
                    CreatedAtUtc = post.CreatedAtUtc,
                    UpdatedAtUtc = post.UpdatedAtUtc,
                    ContentMarkdown = post.ContentMarkdown,

                    Categories = post.PostCategories
                        .Select(postCategory => new PostCategoryRow
                        {
                            Id = postCategory.CategoryId,
                            Name = postCategory.Category.Name,
                            Color = postCategory.Category.Color,
                            Slug = postCategory.Category.Slug,
                            DisplayOrder = postCategory.Category.DisplayOrder
                        })
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            return new PostListResult
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        private static IQueryable<Post> ApplySearch(
            IQueryable<Post> postsQuery,
            string? q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return postsQuery;
            }

            var searchPattern = "%" + q.Trim() + "%";

            return postsQuery.Where(post =>
                EF.Functions.ILike(post.Title, searchPattern) ||
                EF.Functions.ILike(post.Summary, searchPattern));
        }
    }
}