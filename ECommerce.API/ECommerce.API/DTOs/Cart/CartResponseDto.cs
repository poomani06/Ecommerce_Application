namespace ECommerce.API.DTOs
{
  public class CartResponseDto
  {
    public int CartId { get; set; }
    public int UserId { get; set; }
    public List<CartItemResponseDto> CartItems { get; set; } = new();
    public decimal CartTotal => CartItems.Sum(item => item.LineTotal);
  }
}
