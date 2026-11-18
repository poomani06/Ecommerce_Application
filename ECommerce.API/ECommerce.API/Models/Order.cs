namespace ECommerce.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public string PaymentMethod { get; set; } = "CashOnDelivery";

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}