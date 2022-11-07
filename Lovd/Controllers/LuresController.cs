using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lovd.Models;

namespace Lovd.Controllers
{
    public class LuresController : Controller
    {
        private readonly LoveContext _context;

        public LuresController(LoveContext context)
        {
            _context = context;
        }

        // GET: Lures
        public async Task<IActionResult> Index()
        {
              return View(await _context.Lures.ToListAsync());
        }

        // GET: Lures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Lures == null)
            {
                return NotFound();
            }

            var lure = await _context.Lures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lure == null)
            {
                return NotFound();
            }

            return View(lure);
        }

        // GET: Lures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LuresHtml,Date,Title,Announce,PhotoPreview")] Lure lure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lure);
        }

        // GET: Lures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Lures == null)
            {
                return NotFound();
            }

            var lure = await _context.Lures.FindAsync(id);
            if (lure == null)
            {
                return NotFound();
            }
            return View(lure);
        }

        // POST: Lures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LuresHtml,Date,Title,Announce,PhotoPreview")] Lure lure)
        {
            if (id != lure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LureExists(lure.Id))
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
            return View(lure);
        }

        // GET: Lures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Lures == null)
            {
                return NotFound();
            }

            var lure = await _context.Lures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lure == null)
            {
                return NotFound();
            }

            return View(lure);
        }

        // POST: Lures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Lures == null)
            {
                return Problem("Entity set 'LoveContext.Lures'  is null.");
            }
            var lure = await _context.Lures.FindAsync(id);
            if (lure != null)
            {
                _context.Lures.Remove(lure);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LureExists(int id)
        {
          return _context.Lures.Any(e => e.Id == id);
        }
    }
}
