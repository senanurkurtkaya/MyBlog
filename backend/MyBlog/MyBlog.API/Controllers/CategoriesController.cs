using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Categories;
using MyBlog.Business.Categories.Dtos;
using MyBlog.Domain.Models.Posts;

namespace MyBlog.API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController:ControllerBase
    {
        private readonly ICategoryService _categoryService;
      

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto dto , CancellationToken cancellationToken)
        {
            try
            {
                var id = await _categoryService.CreateAsync(dto ,cancellationToken);
                return StatusCode(201, id);
              
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);

            }
            catch(ArgumentException  ex) 
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult>GetAll(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(categories);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult>GetById(Guid id , CancellationToken cancellationToken)
        {
         var category = await   _categoryService.GetByIdAsync(id ,cancellationToken);

            if(category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update(Guid id , UpdateCategoryDto dto,  CancellationToken cancellationToken)
        {

            try
            {
                var updated = await _categoryService.UpdateAsync(id, dto, cancellationToken);

                if (!updated)
                {
                    return NotFound();
                }
                return NoContent();

            }
            catch(InvalidOperationException ex) 
            {
                return Conflict(ex.Message);
            }
            catch(ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>Delete(Guid id , CancellationToken cancellationToken)
        {
         var deleted = await _categoryService.DeleteAsync(id ,cancellationToken);
            if(!deleted)
            {
                return NotFound();
            }
            return NoContent();
     
        }
    }
}
