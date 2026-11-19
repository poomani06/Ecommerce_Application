
using ECommerce.API.Data;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories
{
  public class AuthRepository : IAuthRepository
  {
    private readonly ApplicationDbContext _context;

    public AuthRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
      return await _context.ApplicationUsers
          .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<ApplicationUser> AddUserAsync(ApplicationUser user)
    {
      _context.ApplicationUsers.Add(user);
      await _context.SaveChangesAsync();

      return user;
    }
  }
}
