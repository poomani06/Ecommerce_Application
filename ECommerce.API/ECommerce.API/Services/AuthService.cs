
using ECommerce.API.DTOs.Auth;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.API.Services
{
  public class AuthService : IAuthService
  {
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher;

    public AuthService(
        IAuthRepository authRepository,
        IConfiguration configuration)
    {
      _authRepository = authRepository;
      _configuration = configuration;
      _passwordHasher = new PasswordHasher<ApplicationUser>();
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
      var existingUser = await _authRepository.GetUserByEmailAsync(dto.Email);

      if (existingUser != null)
        return null;

      var user = new ApplicationUser
      {
        Name = dto.Name,
        Email = dto.Email,
        Phone = dto.Phone,
        Role = "Customer"
      };

      user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

      await _authRepository.AddUserAsync(user);

      return GenerateToken(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
      var user = await _authRepository.GetUserByEmailAsync(dto.Email);

      if (user == null)
        return null;

      var result = _passwordHasher.VerifyHashedPassword(
          user,
          user.PasswordHash,
          dto.Password);

      if (result == PasswordVerificationResult.Failed)
        return null;

      return GenerateToken(user);
    }

    private AuthResponseDto GenerateToken(ApplicationUser user)
    {
      var key = _configuration["Jwt:Key"]!;
      var issuer = _configuration["Jwt:Issuer"];
      var audience = _configuration["Jwt:Audience"];
      var expiryMinutes = Convert.ToDouble(
          _configuration["Jwt:ExpiryMinutes"]);

      var claims = new[]
      {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

      var securityKey = new SymmetricSecurityKey(
          Encoding.UTF8.GetBytes(key));

      var credentials = new SigningCredentials(
          securityKey,
          SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: issuer,
          audience: audience,
          claims: claims,
          expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
          signingCredentials: credentials);

      return new AuthResponseDto
      {
        UserId = user.Id,
        Name = user.Name,
        Email = user.Email,
        Role = user.Role,
        Token = new JwtSecurityTokenHandler().WriteToken(token)
      };
    }
  }
}
