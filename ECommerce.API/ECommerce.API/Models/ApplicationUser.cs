namespace ECommerce.API.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";

        // One user has one cart
        public Cart? Cart { get; set; }

        // One user can have many orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}