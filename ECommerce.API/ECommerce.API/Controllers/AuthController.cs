
using ECommerce.API.DTOs.Auth;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
      var result = await _authService.RegisterAsync(dto);

      if (result == null)
        return Conflict(new { message = "Email already exists." });

      return Ok(result);
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
      var result = await _authService.LoginAsync(dto);

      if (result == null)
        return Unauthorized(new { message = "Invalid email or password." });

      return Ok(result);
    }
  }
}
