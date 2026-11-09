using MyBlog.Business.Posts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Posts
{
    public interface IPostService
    {
        Task<Guid> CreateAsync(CreatePostDto dto, CancellationToken cancellationToken = default);

        Task<List<PostListItemDto>> GetPublishedAsync(CancellationToken CancellationToken = default);
        Task<bool> PublishAsync(Guid id ,CancellationToken cancellationToken= default);
        Task<bool> UnpublishAsync(Guid id, CancellationToken cancellationToken = default);

        Task<PostDetailDto?> GetPublishedBySlugAsync(string slug ,CancellationToken cancellationToken = default);
            
    }
}
