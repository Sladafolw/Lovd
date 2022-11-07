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
    public class KindOfFishController : Controller
    {
        private readonly LoveContext _context;

        public KindOfFishController(LoveContext context)
        {
            _context = context;
        }

        // GET: KindOfFish
        public async Task<IActionResult> Index()
        {
              return View(await _context.KindOfFishes.ToListAsync());
        }

        // GET: KindOfFish/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KindOfFishes == null)
            {
                return NotFound();
            }

            var kindOfFish = await _context.KindOfFishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindOfFish == null)
            {
                return NotFound();
            }

            return View(kindOfFish);
        }

        // GET: KindOfFish/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KindOfFish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KindOfFishHtml,Date,PhotoPreview,Title,Announce")] KindOfFish kindOfFish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kindOfFish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kindOfFish);
        }

        // GET: KindOfFish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.KindOfFishes == null)
            {
                return NotFound();
            }

            var kindOfFish = await _context.KindOfFishes.FindAsync(id);
            if (kindOfFish == null)
            {
                return NotFound();
            }
            return View(kindOfFish);
        }

        // POST: KindOfFish/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KindOfFishHtml,Date,PhotoPreview,Title,Announce")] KindOfFish kindOfFish)
        {
            if (id != kindOfFish.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kindOfFish);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KindOfFishExists(kindOfFish.Id))
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
            return View(kindOfFish);
        }

        // GET: KindOfFish/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.KindOfFishes == null)
            {
                return NotFound();
            }

            var kindOfFish = await _context.KindOfFishes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kindOfFish == null)
            {
                return NotFound();
            }

            return View(kindOfFish);
        }

        // POST: KindOfFish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.KindOfFishes == null)
            {
                return Problem("Entity set 'LoveContext.KindOfFishes'  is null.");
            }
            var kindOfFish = await _context.KindOfFishes.FindAsync(id);
            if (kindOfFish != null)
            {
                _context.KindOfFishes.Remove(kindOfFish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KindOfFishExists(int id)
        {
          return _context.KindOfFishes.Any(e => e.Id == id);
        }
    }
}
