using ECommerce.API.DTOs.Order;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
  [Authorize(Policy = "CustomerOnly")]
  [Route("api/[controller]")]
  [ApiController]
  public class OrdersController : ControllerBase
  {
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
      _orderService = orderService;
    }

    private int GetUserId()
    {
      return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
    {
      try
      {
        var order = await _orderService.PlaceOrderAsync(
            GetUserId(), dto.PaymentMethod);

        return Ok(order);
      }
      catch (InvalidOperationException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
      var orders = await _orderService.GetMyOrdersAsync(GetUserId());
      return Ok(orders);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(int orderId)
    {
      var order = await _orderService.GetOrderByIdAsync(
          orderId, GetUserId());

      if (order == null)
        return NotFound(new { message = "Order not found." });

      return Ok(order);
    }
  }
}
