using ECommerce.API.DTOs;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;

namespace ECommerce.API.Services
{
  public class CartService : ICartService
  {
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
      _cartRepository = cartRepository;
      _productRepository = productRepository;
    }

    public async Task<CartResponseDto> GetCartAsync(int userId)
    {
      var cart = await _cartRepository.GetCartByUserIdAsync(userId);

      if (cart == null)
      {
        cart = new Cart { UserId = userId };
        await _cartRepository.AddCartAsync(cart);
        cart = await _cartRepository.GetCartByUserIdAsync(userId);
      }

      return MapToDto(cart!);
    }

    public async Task<CartResponseDto> AddToCartAsync(int userId, int productId, int quantity)
    {
      if (quantity <= 0)
        throw new InvalidOperationException("Quantity must be greater than zero.");

      var product = await _productRepository.GetByIdAsync(productId);

      if (product == null || !product.IsActive)
        throw new KeyNotFoundException("Product not found or inactive.");

      var cart = await GetCartEntityAsync(userId);

      var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
      int newQuantity = (existingItem?.Quantity ?? 0) + quantity;

      if (newQuantity > product.Stock)
        throw new InvalidOperationException("Requested quantity exceeds available stock.");

      if (existingItem != null)
        existingItem.Quantity = newQuantity;
      else
        cart.CartItems.Add(new CartItem
        {
          ProductId = productId,
          Quantity = quantity
        });

      await _cartRepository.SaveChangesAsync();

      return MapToDto(await GetCartEntityAsync(userId));
    }

    public async Task<CartResponseDto> UpdateCartItemAsync(int userId, int cartItemId, int quantity)
    {
      if (quantity <= 0)
        throw new InvalidOperationException("Quantity must be greater than zero.");

      var cart = await GetCartEntityAsync(userId);
      var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

      if (item == null)
        throw new KeyNotFoundException("Cart item not found.");

      if (quantity > item.Product!.Stock)
        throw new InvalidOperationException("Requested quantity exceeds available stock.");

      item.Quantity = quantity;
      await _cartRepository.SaveChangesAsync();

      return MapToDto(await GetCartEntityAsync(userId));
    }

    public async Task<CartResponseDto> RemoveCartItemAsync(int userId, int cartItemId)
    {
      var cart = await GetCartEntityAsync(userId);
      var item = cart.CartItems.FirstOrDefault(ci => ci.CartItemId == cartItemId);

      if (item == null)
        throw new KeyNotFoundException("Cart item not found.");

      cart.CartItems.Remove(item);
      await _cartRepository.SaveChangesAsync();

      return MapToDto(await GetCartEntityAsync(userId));
    }

    private async Task<Cart> GetCartEntityAsync(int userId)
    {
      var cart = await _cartRepository.GetCartByUserIdAsync(userId);

      if (cart == null)
      {
        cart = new Cart { UserId = userId };
        await _cartRepository.AddCartAsync(cart);
        cart = await _cartRepository.GetCartByUserIdAsync(userId);
      }

      return cart!;
    }

    private CartResponseDto MapToDto(Cart cart)
    {
      return new CartResponseDto
      {
        CartId = cart.CartId,
        UserId = cart.UserId,
        CartItems = cart.CartItems.Select(item => new CartItemResponseDto
        {
          CartItemId = item.CartItemId,
          ProductId = item.ProductId,
          ProductName = item.Product!.Name,
          Price = item.Product.Price,
          ImageUrl = item.Product.ImageUrl,
          Quantity = item.Quantity
        }).ToList()
      };
    }
  }
}
