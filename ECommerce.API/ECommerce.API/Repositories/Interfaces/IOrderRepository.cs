using ECommerce.API.Models;

namespace ECommerce.API.Repositories.Interfaces
{
  public interface IOrderRepository
  {
    Task AddOrderAsync(Order order);
    Task<List<Order>> GetOrdersByUserIdAsync(int userId);
    Task<Order?> GetOrderByIdAsync(int orderId, int userId);
    Task SaveChangesAsync();
  }
}
