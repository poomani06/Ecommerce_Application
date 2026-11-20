namespace ECommerce.API.DTOs.Order
{
  public class OrderResponseDto
  {
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public List<OrderItemResponseDto> OrderItems { get; set; } = new();
  }
}
