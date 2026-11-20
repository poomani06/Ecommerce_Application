using ECommerce.API.DTOs.Product;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(new
                {
                    message = "Page number must be positive and page size must be between 1 and 100."
                });
            }

            var products = await _productService.GetAllAsync(
                search, categoryId, pageNumber, pageSize);

            var totalCount = await _productService.GetTotalCountAsync(
                search, categoryId);

            var response = new ProductListResponseDto
            {
                Products = products,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(product);
        }

        [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Create(CreateProductDto dto)
        {
            try
            {
                var product = await _productService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = product.ProductId },
                    product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, UpdateProductDto dto)
        {
            try
            {
                var product = await _productService.UpdateAsync(id, dto);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }

                return Ok(product);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _productService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { message = "Product not found." });
            }

            return Ok(new
            {
                message = "Product deactivated successfully."
            });
        }
    }
}
