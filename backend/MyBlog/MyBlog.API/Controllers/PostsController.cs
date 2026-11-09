using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Posts;
using MyBlog.Business.Posts.Dtos;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService= postService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto dto , CancellationToken cancellationToken)
        {
            try
            {
                var id = await _postService.CreateAsync(dto, cancellationToken);
                return StatusCode(201, id);
            } catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);

            }

        }

        [HttpGet]
        public async Task<IActionResult>GetPublished(CancellationToken cancellationToken)
        {
           var posts = await _postService.GetPublishedAsync(cancellationToken);

           return Ok(posts);
        }

        [HttpGet]
        [Route("{slug}")]
        public async Task<IActionResult> GetBySlug(string slug , CancellationToken cancellationToken)
        {
            var post = await _postService.GetPublishedBySlugAsync(slug , cancellationToken);

            if(post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        [Route("{id:guid}/publish")]
        public async Task<IActionResult> Publish(Guid id , CancellationToken cancellationToken)
        {
           var published = await _postService.PublishAsync(id, cancellationToken);

            if (!published)
            {
                return NotFound();
            }

            return NoContent();
         
        }

        [HttpPost]
        [Route("{id:guid}/unpublish")]
        public async Task<IActionResult>Unpublish(Guid id , CancellationToken cancellationToken)
        {
            var unpublished = await _postService.UnpublishAsync
                (id, cancellationToken);
            if(!unpublished)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>Update(Guid id , UpdatePostDto dto,CancellationToken cancellationToken)
        {
            try
            {
                var updated = await _postService.UpdateAsync(id, dto, cancellationToken);
                if (!updated)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {

                return BadRequest();
            }
            catch(InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
         
        } 
    }    
}
