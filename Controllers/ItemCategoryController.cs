using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Catering.Data;
using Catering.Models;
using Microsoft.AspNetCore.Authorization;

namespace Catering.Controllers
{
    [Authorize(Roles = "Caterer")]
    public class ItemCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public ItemCategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ItemCategory
        public async Task<IActionResult> Index()
        {
            return View(await _context.ItemCategories.ToListAsync());
        }

        // GET: ItemCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.ItemCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return View(itemCategory);
        }

        // GET: ItemCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,Description")] ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        // GET: ItemCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.ItemCategories.FindAsync(id);
            if (itemCategory == null)
            {
                return NotFound();
            }
            return View(itemCategory);
        }

        // POST: ItemCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,Name,Description")] ItemCategory itemCategory)
        {
            if (id != itemCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemCategoryExists(itemCategory.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        // GET: ItemCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.ItemCategories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return View(itemCategory);
        }

        // POST: ItemCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemCategory = await _context.ItemCategories.FindAsync(id);
            if (itemCategory != null)
            {
                _context.ItemCategories.Remove(itemCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemCategoryExists(int id)
        {
            return _context.ItemCategories.Any(e => e.CategoryId == id);
        }
    }
}
