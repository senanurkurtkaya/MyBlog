using MyBlog.Business.Categories.Dtos;
using MyBlog.Domain.Enums;

namespace MyBlog.Business.Posts.Dtos
{
    public class PostListItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }=string.Empty;
        public string Slug { get; set; }= string.Empty;
        public string? Summary { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
        public  PostStatus  Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }
        public int ReadingTimeMinutes { get; set; }
        public List<CategoryDto> Categories { get; set; } = new();
    }
}
