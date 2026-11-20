using ECommerce.API.DTOs.Order;

namespace ECommerce.API.Services.Interfaces
{
  public interface IOrderService
  {
    Task<OrderResponseDto> PlaceOrderAsync(int userId, string paymentMethod);
    Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId);
    Task<OrderResponseDto?> GetOrderByIdAsync(int orderId, int userId);
  }
}
