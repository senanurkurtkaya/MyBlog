using MyBlog.Business.Categories.Dtos;
using MyBlog.Business.Posts.Dtos;
using MyBlog.Data.Repositories.Posts;
using MyBlog.Data.UnitOfWork;
using MyBlog.Domain.Enums;
using MyBlog.Domain.Models.Posts;
using MyBlog.Data.Repositories.Categories;

namespace MyBlog.Business.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork,ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;

        }

        public async Task<Guid> CreateAsync(
            CreatePostDto dto,Guid authorId ,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if(dto.CategoryIds is null)
            {
                throw new ArgumentException("Kategori listesi null olamaz.");
            }
            var categoryIds = dto.CategoryIds.Distinct().ToList();
            var existingCategoryCount = await _categoryRepository.CountByIdsAsync(categoryIds ,cancellationToken);

            if(existingCategoryCount != categoryIds.Count)
            {
                throw new ArgumentException("Bir veya daha fazla kategori bulunamadı");
            }

            if (authorId == Guid.Empty)
            {
                throw new ArgumentException("Yazar kimliği boş olamaz.");
            }

            var (title, slug, summary) = ValidateAndNormalizePostFields(
                dto.Title, dto.Slug, dto.Summary);

            var slugExists = await _postRepository.ExistsBySlugAsync(slug, cancellationToken);
            if (slugExists)
            {
                throw new InvalidOperationException("Bu slug zaten kullanılıyor.");
            }

            var post = new Post(
                title, slug, authorId, summary ?? string.Empty, dto.ContentMarkdown);

            foreach (var categoryId in categoryIds)
            {
                post.AddCategory(categoryId);
            }

            _postRepository.Add(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return post.Id;
        }

        public async Task<List<PostListItemDto>> GetPublishedAsync(
            CancellationToken cancellationToken = default)
        {
            var posts = await _postRepository.GetPublishedAsync(cancellationToken);

            return posts.Select(post => new PostListItemDto
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                PublishedAtUtc = post.PublishedAtUtc,
                Summary = post.Summary,
                Status = post.Status
            }).ToList();
        }

        public async Task<PostDetailDto?> GetPublishedBySlugAsync(
            string slug,
            CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetPublishedBySlugAsync(slug, cancellationToken);

            if (post == null)
            {
                return null;
            }

            return new PostDetailDto
            {
                
                Title = post.Title,
                Slug = post.Slug,
                Summary = post.Summary,
                ContentMarkdown = post.ContentMarkdown,
                PublishedAtUtc = post.PublishedAtUtc,
                Status = post.Status,
                UpdatedAtUtc = post.UpdatedAtUtc,
                Id = post.Id,
                CreatedAtUtc = post.CreatedAtUtc,
                CoverImageId = post.CoverImageId,
                SeoDescription = post.SeoDescription,
                SeoTitle = post.SeoTitle ,
                ReadingTimeMinutes = ReadingTimeCalculator.Calculate(post.ContentMarkdown),
                Categories = post.PostCategories.Select(pc => new CategoryDto
                {
                    Id = pc.CategoryId,
                    Name = pc.Category.Name,
                    Slug = pc.Category.Slug,
                    Color = pc.Category.Color,
                    DisplayOrder = pc.Category.DisplayOrder
                }).ToList()
            };
        }

        public async Task<bool> PublishAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetByIdAsync(id, cancellationToken);

            if (post == null)
            {
                return false;
            }

            post.Publish();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UnpublishAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetByIdAsync(id, cancellationToken);

            if (post == null)
            {
                return false;
            }

            post.Unpublish();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateAsync(
            Guid id,
            UpdatePostDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var post = await _postRepository.GetByIdAsync(id, cancellationToken);

            if (post == null)
            {
                return false;
            }


            if (dto.CategoryIds is null)
            {
                throw new ArgumentException("Kategori listesi null olamaz.");
            }
            var categoryIds = dto.CategoryIds.Distinct().ToList();
            var existingCategoryCount = await _categoryRepository.CountByIdsAsync(categoryIds , cancellationToken);

            if( existingCategoryCount != categoryIds.Count )
            {
                throw new ArgumentException("Bir veya daha fazla kategori bulunamadı.");
            }
   
            var (title, slug, summary) = ValidateAndNormalizePostFields(
                dto.Title, dto.Slug, dto.Summary);

            if (post.Status == PostStatus.Published &&
                string.IsNullOrWhiteSpace(dto.ContentMarkdown))
            {
                throw new ArgumentException("Yayımlanmış bir yazının içeriği boş olamaz.");
            }

            if (post.Slug != slug)
            {
                var slugExists = await _postRepository.ExistsBySlugAsync(slug, cancellationToken);

                if (slugExists)
                {
                    throw new InvalidOperationException("Bu slug zaten kullanılıyor.");
                }

                post.ChangeSlug(slug);
            }

            post.ChangeTitle(title);
            post.ChangeSummary(summary);
            post.ChangeContent(dto.ContentMarkdown);
            var removedCategoryIds = post.PostCategories.Where(postCategory => !categoryIds.Contains(postCategory.CategoryId)).Select(PostCategory => PostCategory.CategoryId).ToList();
            foreach (var categoryId in removedCategoryIds)
            {
                post.RemoveCategory(categoryId);
            }
            foreach (var categoryId in categoryIds)
            {
                post.AddCategory(categoryId);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private static (string Title, string Slug, string? Summary) ValidateAndNormalizePostFields(
            string? title,
            string? slug,
            string? summary)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Yazı başlığı boş olamaz.");
            }

            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Yazı slug'ı boş olamaz.");
            }

            title = title.Trim();
            slug = slug.Trim();
            summary = summary?.Trim();

            if (title.Length > 200)
            {
                throw new ArgumentException("Yazı başlığı en fazla 200 karakter olabilir.");
            }

            if (slug.Length > 200)
            {
                throw new ArgumentException("Yazı slug'ı en fazla 200 karakter olabilir.");
            }

            if (summary != null && summary.Length > 1000)
            {
                throw new ArgumentException("Yazı özeti en fazla 1000 karakter olabilir.");
            }

            return (title, slug, summary);
        }

        public async Task<List<PostListItemDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
          var posts = await _postRepository.GetAllAsync(cancellationToken);
            return posts.Select(post => new PostListItemDto
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                PublishedAtUtc = post.PublishedAtUtc,
                Summary = post.Summary,
                Status = post.Status,
                
            }).ToList();
        }

        public async Task<PostDetailDto?> GetByIdAsync(Guid id ,CancellationToken cancellationToken = default)
        {
            var post = await _postRepository.GetByIdAsync(id, cancellationToken);
            if(post == null)
            {
                return null;
            }
            return new PostDetailDto
            {

                Title = post.Title,
                Slug = post.Slug,
                Summary = post.Summary,
                ContentMarkdown = post.ContentMarkdown,
                PublishedAtUtc = post.PublishedAtUtc,
                Status = post.Status,
                CreatedAtUtc = post.CreatedAtUtc,
                Id = post.Id,
                UpdatedAtUtc = post.UpdatedAtUtc,
                SeoTitle = post.SeoTitle,
                SeoDescription = post.SeoDescription,
                CoverImageId = post.CoverImageId,
                ReadingTimeMinutes = ReadingTimeCalculator.Calculate(post.ContentMarkdown),
                Categories = post.PostCategories.Select(pc => new CategoryDto
                {
                    Id = pc.CategoryId,
                    Name = pc.Category.Name,
                    Slug = pc.Category.Slug,
                    Color = pc.Category.Color,
                    DisplayOrder = pc.Category.DisplayOrder
                }).ToList()

            }; 

        }

        public async Task<AdminPostListResultDto> GetAdminListAsync(AdminPostListQueryDto query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);
           if(query.Page < 1)
            {
                throw new ArgumentException("Sayfa numarası en az 1 olmalıdır");
            }
            if(query.PageSize < 1 || query.PageSize> 100)
            {
                throw new ArgumentException("Sayfa boyutu 1 ile 100 arasında olmalıdır");
            }

            if (query.FromDate.HasValue && query.ToDate.HasValue && query.FromDate.Value > query.ToDate.Value)
            {
                throw new ArgumentException("Başlangıç tarihi bitiş tarihinden sonra olamaz. ");
            }
            if(query.Status.HasValue && !Enum.IsDefined(query.Status.Value))
            {
                throw new ArgumentException("Geçersiz Yazı Durumu");
            }

            var repositoryQuery = new PostListQuery
            {
                Page = query.Page,
                PageSize = query.PageSize,
                Status = query.Status,
                CategoryId = query.CategoryId,
                FromDate = query.FromDate,
                ToDate = query.ToDate,
                Q = query.Q?.Trim()
            };

            var result = await _postRepository.GetAdminListAsync(repositoryQuery, cancellationToken);
            var items = result.Items.Select(post => new PostListItemDto
            {
                Id = post.Id,
                CreatedAtUtc = post.CreatedAtUtc,
                PublishedAtUtc = post.PublishedAtUtc,
                Slug = post.Slug,
                Status = post.Status,
                Summary = post.Summary,
                Title = post.Title,
                UpdatedAtUtc = post.UpdatedAtUtc,

                Categories = post.Categories.Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Color = category.Color,
                    DisplayOrder = category.DisplayOrder,
                    Slug = category.Slug,
                }).ToList()
            }).ToList();

            return new AdminPostListResultDto
            {
                Items = items,
                Page = query.Page,
                PageSize = query.PageSize,
                TotalCount = result.TotalCount,
            };
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationtoken)
        {
          var post =  await _postRepository.GetByIdAsync(id, cancellationtoken);

            if(post == null)
            {
                return false;
            }
            _postRepository.Remove(post);

            await _unitOfWork.SaveChangesAsync(cancellationtoken);
            return true;
                
        }

        public async Task<PublicPostListResultDto> GetPublicListAsync(PublicPostListQueryDto query, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(query);
            if (query.Page < 1)
            {
                throw new ArgumentException("Sayfa numarası en az 1 olmalıdır.");
            }
            if(query.PageSize < 1 || query.PageSize > 100)
            {
                throw new ArgumentException("Sayfa boyutu 1 ile 100 arasında olmalıdır.");
            }

            var repositoryQuery = new PostListQuery
            {
                Page = query.Page,
                PageSize = query.PageSize,
                CategorySlug = query.Category?.Trim(),
                Q = query.Q?.Trim()
            };
            var result = await _postRepository.GetPublicListAsync(repositoryQuery, cancellationToken);
            var items = result.Items.Select(post => new PostListItemDto
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                Summary = post.Summary,
                Status = post.Status,
                PublishedAtUtc = post.PublishedAtUtc,
                CreatedAtUtc = post.CreatedAtUtc,
                UpdatedAtUtc = post.UpdatedAtUtc,
                ReadingTimeMinutes = ReadingTimeCalculator.Calculate(post.ContentMarkdown),

                Categories = post.Categories.Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Color = category.Color,
                    DisplayOrder = category.DisplayOrder,
                    Slug = category.Slug,
                }).ToList()

            }).ToList();
            return new PublicPostListResultDto
            {
                Items = items,
                TotalCount = result.TotalCount,
                Page = query.Page,
                PageSize = query.PageSize,
            };
        }
    }
}