using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catering.Data;
using Catering.Models;
using Catering.Models.ViewModels;

namespace Catering.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CatererController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CatererController(AppDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Caterer/Search
        [HttpGet]
        public async Task<IActionResult> Search(CatererSearchViewModel vm)
        {
            
            var catererRole = await _db.Roles
                .Where(r => r.Name == "Caterer")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            
            var catererIds = await _db.UserRoles
                .Where(ur => ur.RoleId == catererRole)
                .Select(ur => ur.UserId)
                .ToListAsync();

           
            var query = _db.Users
                .Where(u => catererIds.Contains(u.Id))
                .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(vm.Location))
            {
                query = query.Where(u => u.Address.Contains(vm.Location));
            }

           
            if (vm.NumGuests.HasValue)
            {
                query = query.Where(u => u.Capacity >= vm.NumGuests.Value);
            }

           
            vm.Results = await query
                .Select(u => new CatererListItemViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Address = u.Address,
                    MinGuests = u.Capacity
                })
                .ToListAsync();

            return View(vm);
        }

        // GET: /Caterer/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();
            var caterer = await _userManager.FindByIdAsync(id);
            if (caterer == null) return NotFound();

            
            var items = await _db.Items
                                .Include(i => i.Category)
                                .Where(i => i.Category != null) 
                                .ToListAsync();

            var vm = new CatererDetailsViewModel
            {
                Caterer = caterer,
                Items = items
            };
            return View(vm);
        }




    }
}
