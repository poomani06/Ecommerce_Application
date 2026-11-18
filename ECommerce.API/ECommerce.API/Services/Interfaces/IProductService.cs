using ECommerce.API.DTOs.Product;

namespace ECommerce.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductResponseDto>> GetAllAsync(
            string? search,
            int? categoryId,
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync(
            string? search,
            int? categoryId);

        Task<ProductResponseDto?> GetByIdAsync(int id);

        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);

        Task<ProductResponseDto?> UpdateAsync(
            int id,
            UpdateProductDto dto);

        Task<bool> DeleteAsync(int id);
    }
}