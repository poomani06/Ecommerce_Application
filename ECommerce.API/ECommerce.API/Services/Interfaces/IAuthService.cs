
using ECommerce.API.DTOs.Auth;

namespace ECommerce.API.Services.Interfaces
{
  public interface IAuthService
  {
    Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
  }
}
