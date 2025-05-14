using System.ComponentModel.DataAnnotations;

namespace Catering.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive.")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public ItemCategory Category { get; set; }
    }
}