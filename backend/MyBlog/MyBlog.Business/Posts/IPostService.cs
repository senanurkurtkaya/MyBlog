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


    }
}
