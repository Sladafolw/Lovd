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
    public class BaitsController : Controller
    {
        private readonly LoveContext _context;

        public BaitsController(LoveContext context)
        {
            _context = context;
        }

        // GET: Baits
        public async Task<IActionResult> Index()
        {
              return View(await _context.Baits.ToListAsync());
        }

        // GET: Baits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Baits == null)
            {
                return NotFound();
            }

            var bait = await _context.Baits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bait == null)
            {
                return NotFound();
            }

            return View(bait);
        }

        // GET: Baits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Baits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BaitsHtml,Date,PhotoPreview,Title,Announce")] Bait bait)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bait);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bait);
        }

        // GET: Baits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Baits == null)
            {
                return NotFound();
            }

            var bait = await _context.Baits.FindAsync(id);
            if (bait == null)
            {
                return NotFound();
            }
            return View(bait);
        }

        // POST: Baits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BaitsHtml,Date,PhotoPreview,Title,Announce")] Bait bait)
        {
            if (id != bait.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bait);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaitExists(bait.Id))
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
            return View(bait);
        }

        // GET: Baits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Baits == null)
            {
                return NotFound();
            }

            var bait = await _context.Baits
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bait == null)
            {
                return NotFound();
            }

            return View(bait);
        }

        // POST: Baits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Baits == null)
            {
                return Problem("Entity set 'LoveContext.Baits'  is null.");
            }
            var bait = await _context.Baits.FindAsync(id);
            if (bait != null)
            {
                _context.Baits.Remove(bait);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaitExists(int id)
        {
          return _context.Baits.Any(e => e.Id == id);
        }
    }
}
