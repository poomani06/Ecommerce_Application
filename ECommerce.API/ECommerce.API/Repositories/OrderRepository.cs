using ECommerce.API.Data;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
      await _context.Orders.AddAsync(order);
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
    {
      return await _context.Orders
          .Include(o => o.OrderItems)
          .ThenInclude(oi => oi.Product)
          .Where(o => o.UserId == userId)
          .OrderByDescending(o => o.OrderDate)
          .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId, int userId)
    {
      return await _context.Orders
          .Include(o => o.OrderItems)
          .ThenInclude(oi => oi.Product)
          .FirstOrDefaultAsync(o =>
              o.OrderId == orderId && o.UserId == userId);
    }

    public async Task SaveChangesAsync()
    {
      await _context.SaveChangesAsync();
    }
  }
}
