using System.ComponentModel.DataAnnotations;

namespace Catering.Models
{
    public class ItemCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
