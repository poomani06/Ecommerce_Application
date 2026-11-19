
using ECommerce.API.Models;

namespace ECommerce.API.Repositories.Interfaces
{
  public interface IAuthRepository
  {
    Task<ApplicationUser?> GetUserByEmailAsync(string email);
    Task<ApplicationUser> AddUserAsync(ApplicationUser user);
  }
}
