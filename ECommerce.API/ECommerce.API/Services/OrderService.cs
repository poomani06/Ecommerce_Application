using ECommerce.API.DTOs.Order;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;

namespace ECommerce.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }

        public async Task<OrderResponseDto> PlaceOrderAsync(int userId, string paymentMethod)
        {
            if (paymentMethod != "CashOnDelivery")
                throw new InvalidOperationException("Only CashOnDelivery is supported.");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Your cart is empty.");

            foreach (var item in cart.CartItems)
            {
                if (!item.Product!.IsActive)
                    throw new InvalidOperationException(
                        $"{item.Product.Name} is no longer available.");

                if (item.Quantity > item.Product.Stock)
                    throw new InvalidOperationException(
                        $"Insufficient stock for {item.Product.Name}.");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                PaymentMethod = paymentMethod,
                TotalAmount = cart.CartItems.Sum(
                    item => item.Product!.Price * item.Quantity),

              OrderItems = cart.CartItems.Select(item => new OrderItem
              {
                ProductId = item.ProductId,
                Product = item.Product!,
                Quantity = item.Quantity,
                UnitPrice = item.Product!.Price
              }).ToList()
            };

            foreach (var item in cart.CartItems)
            {
                item.Product!.Stock -= item.Quantity;
            }

            cart.CartItems.Clear();

            await _orderRepository.AddOrderAsync(order);
            await _orderRepository.SaveChangesAsync();

            return MapToDto(order);
        }

        public async Task<List<OrderResponseDto>> GetMyOrdersAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);

            return orders.Select(MapToDto).ToList();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId, userId);

            return order == null ? null : MapToDto(order);
        }

        private OrderResponseDto MapToDto(Order order)
        {
            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,

                OrderItems = order.OrderItems.Select(item => new OrderItemResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product!.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList()
            };
        }
    }
}
