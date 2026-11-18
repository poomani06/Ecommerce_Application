namespace ECommerce.API.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // One category has many products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}