
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
  [Authorize(Policy = "CustomerOnly")]
  [Route("api/[controller]")]
  [ApiController]
  public class CartController : ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
      _cartService = cartService;
    }

    private int GetUserId()
    {
      return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
      var cart = await _cartService.GetCartAsync(GetUserId());
      return Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart(int productId, int quantity)
    {
      try
      {
        var cart = await _cartService.AddToCartAsync(
            GetUserId(), productId, quantity);

        return Ok(cart);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }

    [HttpPut("items/{cartItemId}")]
    public async Task<IActionResult> UpdateCartItem(
        int cartItemId, int quantity)
    {
      try
      {
        var cart = await _cartService.UpdateCartItemAsync(
            GetUserId(), cartItemId, quantity);

        return Ok(cart);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveCartItem(int cartItemId)
    {
      try
      {
        var cart = await _cartService.RemoveCartItemAsync(
            GetUserId(), cartItemId);

        return Ok(cart);
      }
      catch (KeyNotFoundException ex)
      {
        return NotFound(new { message = ex.Message });
      }
    }
  }
}
