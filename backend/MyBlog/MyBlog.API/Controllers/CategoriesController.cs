using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Categories;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(
                cancellationToken);

            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetByIdAsync(
                id, cancellationToken);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
    }
}