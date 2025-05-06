
namespace Catering.Models
{ 
    public class Booking
{
    public int BookingId { get; set; }
    public string CustomerId { get; set; }
    public ApplicationUser Customer { get; set; }

    public string CatererId { get; set; }
    public ApplicationUser Caterer { get; set; }

    public DateTime Date { get; set; }
    public string Venue { get; set; }
    public int NumGuests { get; set; }
    public decimal InvoiceAmount { get; set; }
    public BookingStatus Status { get; set; }
     public ICollection<BookingItem> BookingItems { get; set; }
    public Invoice Invoice { get; set; }
}
}