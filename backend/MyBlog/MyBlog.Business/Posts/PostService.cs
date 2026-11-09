using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyBlog.Business.Posts.Dtos;
using MyBlog.Data.Repositories.Posts;
using MyBlog.Data.UnitOfWork;
using MyBlog.Domain.Enums;
using MyBlog.Domain.Models.Posts;

namespace MyBlog.Business.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateAsync(
            CreatePostDto dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (dto.AuthorId == Guid.Empty)
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
                title, slug, dto.AuthorId, summary ?? string.Empty, dto.ContentMarkdown);

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
                Title = post.Title,
                Slug = post.Slug,
                PublishedAtUtc = post.PublishedAtUtc,
                Summary = post.Summary
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
                PublishedAtUtc = post.PublishedAtUtc
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
    }
}