using ECommerce.API.DTOs.Category;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace ECommerce.API.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }

            return Ok(category);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
        {
            try
            {
                var category = await _categoryService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = category.CategoryId },
                    category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategoryDto dto)
        {
            try
            {
                var category = await _categoryService.UpdateAsync(id, dto);

                if (category == null)
                {
                    return NotFound(new { message = "Category not found." });
                }

                return Ok(category);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool deleted = await _categoryService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = "Category not found." });
                }

                return Ok(new { message = "Category deleted successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
