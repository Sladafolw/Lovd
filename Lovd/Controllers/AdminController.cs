using Lovd.Data;
using Lovd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Lovd.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager <IdentityRole> roleManager;
        private readonly LoveContext _context;
      
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, LoveContext context) 
        {this.roleManager = roleManager;
            _context=context;
           _userManager=userManager;
          

        }
      
        // GET: Admin/Details/5
        public ActionResult Admin()
        {
            return View();
        }
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateRole()
        { return View(); }

            // GET: Admin/Create
            [HttpPost]
        public async Task <ActionResult> CreateRole(string roleName)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) 
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Acces()
        { return View(); }

        public async Task<IActionResult> IndexNews()
        {
            var loveContext = _context.News.Include(n => n.User);
            return View(await loveContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> DetailsNews(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.IdNews == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult CreateNews()
        {
            ViewBag.userId = _userManager.GetUserId(HttpContext.User);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNews([Bind("IdNews,NewsHtml,Likes,DisLikes,UserId,DateNews,Title,Announce")] News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", news.UserId);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> EditNews(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", news.UserId);
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditNews(int id, [Bind("IdNews,NewsHtml,Likes,DisLikes,UserId,DateNews,Title,Announce")] News news)
        {
            if (id != news.IdNews)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.IdNews))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexNews));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", news.UserId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> DeleteNews(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.IdNews == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                return Problem("Entity set 'LoveContext.News'  is null.");
            }
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.IdNews == id);
        }

    }

}