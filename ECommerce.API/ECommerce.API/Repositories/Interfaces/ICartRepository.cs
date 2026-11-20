
using ECommerce.API.Models;

namespace ECommerce.API.Repositories.Interfaces
{
  public interface ICartRepository
  {
    Task<Cart?> GetCartByUserIdAsync(int userId);

    Task AddCartAsync(Cart cart);

    Task SaveChangesAsync();
  }
}
