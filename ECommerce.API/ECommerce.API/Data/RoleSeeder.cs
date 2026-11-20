using ECommerce.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Data
{
  public static class RoleSeeder
  {
    public static async Task SeedAdminAsync(
        ApplicationDbContext context,
        IConfiguration configuration)
    {
      var adminEmail = configuration["AdminSeed:Email"];

      var existingAdmin = await context.ApplicationUsers
          .FirstOrDefaultAsync(u => u.Email == adminEmail);

      if (existingAdmin != null)
        return;

      var admin = new ApplicationUser
      {
        Name = configuration["AdminSeed:Name"]!,
        Email = adminEmail!,
        Phone = configuration["AdminSeed:Phone"]!,
        Role = "Admin"
      };

      var passwordHasher = new PasswordHasher<ApplicationUser>();

      admin.PasswordHash = passwordHasher.HashPassword(
          admin,
          configuration["AdminSeed:Password"]!);

      context.ApplicationUsers.Add(admin);

      await context.SaveChangesAsync();
    }
  }
}
