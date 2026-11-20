using ECommerce.API.DTOs;

namespace ECommerce.API.Services.Interfaces
{
  public interface ICartService
  {
    Task<CartResponseDto> GetCartAsync(int userId);
    Task<CartResponseDto> AddToCartAsync(int userId, int productId, int quantity);
    Task<CartResponseDto> UpdateCartItemAsync(int userId, int cartItemId, int quantity);
    Task<CartResponseDto> RemoveCartItemAsync(int userId, int cartItemId);
  }
}
