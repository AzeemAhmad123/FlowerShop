using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = string.Empty;

        [StringLength(50)]
        public string Color { get; set; } = string.Empty;
    }
}


