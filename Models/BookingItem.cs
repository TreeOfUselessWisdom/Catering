
namespace Catering.Models
{
    public class BookingItem
    {
        public int BookingItemId { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}