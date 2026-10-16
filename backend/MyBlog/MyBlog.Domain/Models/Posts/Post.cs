using MyBlog.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Domain.Models.Posts
{
    public class Post
    {
        private readonly List<PostCategory> _postCategories=new List<PostCategory>();
        public IReadOnlyCollection<PostCategory> PostCategories => _postCategories.AsReadOnly();
        public Guid Id { get; private set; }

        public string Slug { get; private set; }
        public string Title { get; private set; }

        public string Summary { get; private set; }

        public string ContentMarkdown { get; private set; }

        public DateTime CreatedAtUtc { get; private set; } 

        public DateTime? UpdatedAtUtc { get; private set; }

        public DateTime? PublishedAtUtc { get; private set; }

        public Guid? CoverImageId { get; private set; }

        public Guid AuthorId { get; private set; }

        public PostStatus Status { get; private set; }

        public string? SeoTitle { get; private set; }

        public string? SeoDescription { get; private set; }

        public Post(string title , string slug , Guid authorId,string? summary, string? contentMarkdown)
        {

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Başlık Boş Olamaz");
            }

            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Slug boş olamaz");
            }

            if(Guid.Empty == authorId)
            {
                throw new ArgumentException("Yazar Kimliği Boş Olamaz.");
            } 
            Title = title.Trim();
            Slug = slug.Trim();
            AuthorId = authorId;
            Id = Guid.NewGuid();
            CreatedAtUtc = DateTime.UtcNow;
            Status = PostStatus.Draft;
            Summary = summary?.Trim() ?? string.Empty;
            ContentMarkdown = contentMarkdown ?? string.Empty;
          
        }

        public void ChangeTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("Yeni başlık boş olamaz.");
            }
            Title = newTitle.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeSummary(string? newSummary)
        {
            Summary = newSummary?.Trim() ?? string.Empty;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeContent(string? newContentMarkdown)
        {
            ContentMarkdown = newContentMarkdown ?? string.Empty;
            UpdatedAtUtc = DateTime.UtcNow;
        }
        
        public void Publish()
        {
            if (Status == PostStatus.Published)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(ContentMarkdown))
            {
                throw new InvalidOperationException("İçeriği boş olan yazı yayımlanamaz.");
            }
            Status = PostStatus.Published;
            PublishedAtUtc = DateTime.UtcNow;
            UpdatedAtUtc = PublishedAtUtc;

        }

        public void Unpublish()
        {
            if(Status == PostStatus.Draft)
            {
                return ;
            }
            Status = PostStatus.Draft;
            UpdatedAtUtc= DateTime.UtcNow;
            PublishedAtUtc = null;
        }

        public void ChangeCoverImage(Guid? coverImageId)
        {
            if(coverImageId == Guid.Empty)
            {
                throw new ArgumentException("Kapak görseli kimliği boş GUID olamaz.");
            }
            CoverImageId = coverImageId;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeSeo(string? seoTitle ,string? seoDescription)
        {
            SeoTitle = seoTitle?.Trim();
            SeoDescription = seoDescription?.Trim();
            UpdatedAtUtc =DateTime.UtcNow;

        }

        public void ChangeSlug(string newSlug)
        {
            if(string.IsNullOrWhiteSpace(newSlug))
            {
                throw new ArgumentException(" Yeni slug boş olamaz.");
            }
            Slug = newSlug.Trim();
            UpdatedAtUtc = DateTime.UtcNow; 

        }
        public void AddCategory(Guid categoryId)
        {
            if(categoryId == Guid.Empty)
            {
                throw new ArgumentException("Kategori Kimliği Boş Olamaz",nameof(categoryId));
            }
            if(_postCategories.Any(x => x.CategoryId == categoryId))
            {

                return;
            }
            var postCategory = new PostCategory(Id, categoryId);
            _postCategories.Add(postCategory);
            UpdatedAtUtc= DateTime.UtcNow;

        }
        public void RemoveCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new ArgumentException("Kategori Kimliği boş olamaz.", nameof(categoryId));
            }

            var postCategory = _postCategories.FirstOrDefault(x => x.CategoryId == categoryId);

            if(postCategory is null)
            {
                return;
            }
            _postCategories.Remove(postCategory);
            UpdatedAtUtc = DateTime.UtcNow;
        }
    }
}
