using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Posts;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPublished(
            CancellationToken cancellationToken)
        {
            var posts = await _postService.GetPublishedAsync(cancellationToken);
            return Ok(posts);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetBySlug(
            string slug,
            CancellationToken cancellationToken)
        {
            var post = await _postService.GetPublishedBySlugAsync(
                slug, cancellationToken);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }
    }
}