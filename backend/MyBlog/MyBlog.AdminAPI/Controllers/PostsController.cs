using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Posts;
using MyBlog.Business.Posts.Dtos;

namespace MyBlog.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/posts")]
    [Authorize(Roles = "Admin")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var posts = await _postService.GetAllAsync(cancellationToken);
            return Ok(posts);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var post = await _postService.GetByIdAsync(id, cancellationToken);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePostDto dto, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _postService.CreateAsync(dto, cancellationToken);
                return CreatedAtAction(nameof(GetById), new { id }, new { id });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id , UpdatePostDto dto , CancellationToken cancellationToken)
        {
            try
            {
              var updated = await  _postService.UpdateAsync(id, dto, cancellationToken);
              if (!updated)
            {
                return NotFound();
            }
            return NoContent();

            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:guid}/publish")]
        public async Task<IActionResult> PublishAsync(Guid id , CancellationToken cancellationToken)
        {
            try
            {
            var published = await _postService.PublishAsync(id , cancellationToken);
            if (!published)
            {
                return NotFound();
            }
            return NoContent();

            }
            catch (InvalidOperationException ex)
            {
               return Conflict(ex.Message);
            }
        }

        [HttpPost("{id:guid}/unpublish")]
        public async Task<IActionResult> UnPublishAsync(Guid id ,CancellationToken cancellationToken)
        {
          
            var unpublished = await _postService.UnpublishAsync(id , cancellationToken);
            if (!unpublished)
            {
                return NotFound();
            }
            return NoContent();
         
        }

    }
}
