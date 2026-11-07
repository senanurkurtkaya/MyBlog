using MyBlog.Business.Posts.Dtos;
using MyBlog.Data.Repositories.Categories;
using MyBlog.Data.Repositories.Posts;
using MyBlog.Data.UnitOfWork;
using MyBlog.Domain.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IPostRepository postRepository , IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Guid> CreateAsync(CreatePostDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new ArgumentException("yazı başlığı boş olamaz");
            }
            if (string.IsNullOrWhiteSpace(dto.Slug))
            {
                throw new ArgumentException("yazı slug'ı boş olamaz");
                
            }
            if(dto.AuthorId == Guid.Empty)
            {
                throw new ArgumentException("Yazar Kimliği boş olamaz");
            }
            
            var title = dto.Title.Trim();
            var slug = dto.Slug.Trim();
            var summary = dto.Summary?.Trim() ?? string.Empty;
            

            if (title.Length > 200)
            {
                throw new ArgumentException("Yazı başlığı en fazla 200 karakter olabilir");
            }
            if (slug.Length > 200)
            {
                throw new ArgumentException("Yazı slug'ı en fazla 200 karakter olabilir");
            }
            if(summary.Length > 1000)
            {
                throw new ArgumentException("Yazı özeti en fazla 1000 karakter olabilir");
            }
            var slugExists = await _postRepository.ExistsBySlugAsync(slug ,cancellationToken);
            if (slugExists)
            {
                throw new InvalidOperationException("Bu slug zaten kullanılıyor.");
            }

            var post = new Post(title ,slug ,dto.AuthorId , summary, dto.ContentMarkdown);
            _postRepository.Add(post);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return post.Id;
        }
    }
}
