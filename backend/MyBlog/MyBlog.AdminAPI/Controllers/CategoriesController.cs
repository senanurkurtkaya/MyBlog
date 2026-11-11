using Microsoft.AspNetCore.Mvc;
using MyBlog.Business.Categories;
using MyBlog.Business.Categories.Dtos;
using MyBlog.Domain.Models.Categories;
using System.Security.Cryptography.X509Certificates;

namespace MyBlog.AdminAPI.Controllers
{

    [ApiController]
    [Route("api/categories")]
    public class CategoriesController :ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
         var categories = await  _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id ,CancellationToken cancellationToken)
        {
          var category =await  _categoryService.GetByIdAsync(id ,cancellationToken);
            if(category == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(category);
            }
        }

        [HttpPost]
        public async Task<IActionResult>Create(CreateCategoryDto dto , CancellationToken cancellationToken)
        {
            try
            {
                var id = await _categoryService.CreateAsync(dto, cancellationToken);
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
        public async Task<IActionResult> Update(Guid id ,UpdateCategoryDto dto , CancellationToken cancellationToken)
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
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id , CancellationToken cancellationToken)
        {
            var deleted = await _categoryService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
