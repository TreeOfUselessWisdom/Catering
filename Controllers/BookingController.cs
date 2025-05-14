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

    // POST: /Booking/Cancel/5
    [HttpPost]
    [Authorize(Roles = "Customer")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var user = await _users.GetUserAsync(User);
        var booking = await _db.Bookings
            .Include(b => b.Caterer)
            .FirstOrDefaultAsync(b => b.BookingId == id && b.CustomerId == user.Id);

        if (booking == null) return NotFound();

        // enforce 7‑day rule
        if ((booking.Date - DateTime.Today).TotalDays < 7)
        {
            ModelState.AddModelError(string.Empty, "You can only cancel at least 7 days before the event.");
            return RedirectToAction(nameof(MyBookings));
        }

        // apply cancellation fee from caterer profile
        var feePercent = booking.Caterer.CancellationFeePercent;
        var refund = booking.InvoiceAmount * (1 - feePercent / 100m);

        booking.Status = BookingStatus.Cancelled;
        // Optionally store refund somewhere: booking.Invoice.Amount = refund; …
        await _db.SaveChangesAsync();

        TempData["Message"] = $"Booking #{id} cancelled. Refund: {refund:C} (fee {feePercent}%).";
        return RedirectToAction(nameof(MyBookings));
    }


    // GET: /Booking/Index
    [HttpGet]
    [Authorize(Roles = "Caterer")]
    public async Task<IActionResult> Index(string status, DateTime? from, DateTime? to)
    {
        var user = await _users.GetUserAsync(User);
        var q = _db.Bookings
            .Where(b => b.CatererId == user.Id)
            .Include(b => b.Customer)
            .AsQueryable();

        if (Enum.TryParse<BookingStatus>(status, out var st))
            q = q.Where(b => b.Status == st);
        if (from.HasValue) q = q.Where(b => b.Date >= from.Value);
        if (to.HasValue) q = q.Where(b => b.Date <= to.Value);

        return View(await q.ToListAsync());
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

    // GET: /Booking/Details/5    (for caterer)
    [HttpGet]
    [Authorize(Roles = "Caterer")]
    public async Task<IActionResult> Details(int id)
    {
        var user = await _users.GetUserAsync(User);
        var booking = await _db.Bookings
            .Include(b => b.Customer)
            .Include(b => b.BookingItems).ThenInclude(bi => bi.Item)
            .FirstOrDefaultAsync(b => b.BookingId == id && b.CatererId == user.Id);
        if (booking == null) return NotFound();
        return View(booking);
    }


}
