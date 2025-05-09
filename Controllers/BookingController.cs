using Catering.Data;
using Catering.Models;
using Catering.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Customer")]
public class BookingController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _users;
    public BookingController(AppDbContext db, UserManager<ApplicationUser> users)
    {
        _db = db; _users = users;
    }

    // GET: /Booking/Create
    [HttpGet]
    public async Task<IActionResult> Create(string catererId, int itemId)
    {
        var user = await _users.GetUserAsync(User);
        var caterer = await _users.FindByIdAsync(catererId);
        var item = await _db.Items.FindAsync(itemId);

        var vm = new BookingCreateViewModel
        {
            CatererId = catererId,
            CatererName = caterer.FullName,
            ItemId = itemId,
            ItemName = item.Name,
            Price = item.Price,
            Date = DateTime.Today.AddDays(7),  
            Venue = "",
            NumGuests = 50
        };
        return View(vm);
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Create(BookingCreateViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _users.GetUserAsync(User);
        var booking = new Booking
        {
            CustomerId = user.Id,
            CatererId = vm.CatererId,
            Date = vm.Date,
            Venue = vm.Venue,
            NumGuests = vm.NumGuests,
            InvoiceAmount = vm.Price * vm.NumGuests,
            Status = BookingStatus.Pending
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        
        _db.BookingItems.Add(new BookingItem
        {
            BookingId = booking.BookingId,
            ItemId = vm.ItemId,
            Quantity = vm.NumGuests,
            Price = vm.Price
        });
        await _db.SaveChangesAsync();

        return RedirectToAction("MyBookings");
    }

    // GET: /Booking/MyBookings
    [HttpGet]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> MyBookings()
    {
        var user = await _users.GetUserAsync(User);
        var bookings = await _db.Bookings
            .Where(b => b.CustomerId == user.Id)
            .Include(b => b.Invoice)
            .ToListAsync();
        return View(bookings);
    }

    // GET: /Booking/Index
    [HttpGet]
    [Authorize(Roles = "Caterer")]
    public async Task<IActionResult> Index()
    {
        var user = await _users.GetUserAsync(User);
        var list = await _db.Bookings
            .Where(b => b.CatererId == user.Id)
            .Include(b => b.Customer)
            .ToListAsync();
        return View(list);
    }

    [HttpGet]
    [Authorize(Roles = "Caterer")]
    public async Task<IActionResult> UpdateStatus(int id)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b == null) return NotFound();
        b.Status = BookingStatus.Confirmed;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}
