using System.ComponentModel.DataAnnotations;

public class BookingCreateViewModel
{
    public string CatererId { get; set; }
    public string CatererName { get; set; }

    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public decimal Price { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    public string Venue { get; set; }

    [Range(50, int.MaxValue)]
    public int NumGuests { get; set; }
}
