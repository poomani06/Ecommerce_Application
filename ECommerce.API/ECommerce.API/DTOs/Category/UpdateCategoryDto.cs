using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}