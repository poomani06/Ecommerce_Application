
using ECommerce.API.Data;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories
{
  public class CartRepository : ICartRepository
  {
    private readonly ApplicationDbContext _context;

    public CartRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(int userId)
    {
      return await _context.Carts
          .Include(c => c.CartItems)
          .ThenInclude(ci => ci.Product)
          .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddCartAsync(Cart cart)
    {
      await _context.Carts.AddAsync(cart);
      await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
