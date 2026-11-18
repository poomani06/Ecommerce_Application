using ECommerce.API.Models;

namespace ECommerce.API.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync(
            string? search,
            int? categoryId,
            int pageNumber,
            int pageSize);

        Task<int> GetTotalCountAsync(
            string? search,
            int? categoryId);

        Task<Product?> GetByIdAsync(int id);

        Task<bool> CategoryExistsAsync(int categoryId);

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);
    }
}