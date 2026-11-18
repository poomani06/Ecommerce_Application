using ECommerce.API.DTOs.Product;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;

namespace ECommerce.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductResponseDto>> GetAllAsync(
            string? search,
            int? categoryId,
            int pageNumber,
            int pageSize)
        {
            var products = await _productRepository.GetAllAsync(
                search, categoryId, pageNumber, pageSize);

            return products.Select(p => MapToDto(p)).ToList();
        }

        public async Task<int> GetTotalCountAsync(
            string? search,
            int? categoryId)
        {
            return await _productRepository.GetTotalCountAsync(
                search, categoryId);
        }

        public async Task<ProductResponseDto?> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            return MapToDto(product);
        }

        public async Task<ProductResponseDto> CreateAsync(
            CreateProductDto dto)
        {
            string name = dto.Name.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name is required.");
            }

            if (dto.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            if (dto.Stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative.");
            }

            if (!await _productRepository.CategoryExistsAsync(dto.CategoryId))
            {
                throw new ArgumentException("Category does not exist.");
            }

            var product = new Product
            {
                Name = name,
                Description = dto.Description?.Trim(),
                Price = dto.Price,
                Stock = dto.Stock,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                IsActive = true
            };

            await _productRepository.AddAsync(product);

            var savedProduct = await _productRepository.GetByIdAsync(
                product.ProductId);

            return MapToDto(savedProduct!);
        }

        public async Task<ProductResponseDto?> UpdateAsync(
            int id, UpdateProductDto dto)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            string name = dto.Name.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Product name is required.");
            }

            if (dto.Price <= 0)
            {
                throw new ArgumentException("Price must be greater than zero.");
            }

            if (dto.Stock < 0)
            {
                throw new ArgumentException("Stock cannot be negative.");
            }

            if (!await _productRepository.CategoryExistsAsync(dto.CategoryId))
            {
                throw new ArgumentException("Category does not exist.");
            }

            product.Name = name;
            product.Description = dto.Description?.Trim();
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.ImageUrl = dto.ImageUrl;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;

            await _productRepository.UpdateAsync(product);

            return MapToDto(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return false;
            }

            product.IsActive = false;

            await _productRepository.UpdateAsync(product);

            return true;
        }

        private static ProductResponseDto MapToDto(Product product)
        {
            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty
            };
        }
    }
}