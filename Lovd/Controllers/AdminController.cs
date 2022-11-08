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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly LoveContext _context;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, LoveContext context)
        {
            this.roleManager = roleManager;
            _context = context;
            _userManager = userManager;


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
        public async Task<ActionResult> CreateRole(string roleName)
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
        [HttpGet]
        public async Task<ActionResult> Test()
        { return View(); }
        [HttpPost]
        public async Task<ActionResult> Test(IFormFile filemy)
        {
            var a = filemy.FileName;
            return View();
        }
        public async Task<IActionResult> IndexArticles()
        {
            var loveContext = _context.Articles.Include(n => n.User);
            return View(await loveContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> DetailsArticles(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var news = await _context.Articles
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.IdArticle == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult CreateArticles()
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
        public async Task<IActionResult> CreateArticles([Bind("IdArticle,ArticleHtml,Likes,DisLikes,UserId,DateNews,Title,Announce")] Article article, IFormFile PhotoPreview)
        {
            if (ModelState.IsValid)
            {
                if (PhotoPreview != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(PhotoPreview.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)PhotoPreview.Length);
                    }
                    // установка массива байтов
                    article.PhotoPreview = imageData;
                }


                _context.Add(article);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexArticles));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", article.UserId);
            return RedirectToAction("IndexArticles");
        }

        // GET: News/Edit/5
        public async Task<IActionResult> EditArticles(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", article.UserId);
            return View(article);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticles(int id, [Bind("IdArticle,ArticleHtml,Likes,DisLikes,UserId,DateNews,Title,PhotoPreview,Announce")] Article articles)
        {
            if (id != articles.IdArticle)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticlesExists(articles.IdArticle))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexArticles));
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", articles.UserId);
            return View(articles);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> DeleteArticles(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var articles = await _context.Articles
                .Include(n => n.User)
                .FirstOrDefaultAsync(m => m.IdArticle == id);
            if (articles == null)
            {
                return NotFound();
            }

            return View(articles);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdArticle)
        {
            if (_context.Articles == null)
            {
                return Problem("Entity set 'LoveContext.News'  is null.");
            }
            var articles = await _context.Articles.FindAsync(IdArticle);
            if (articles != null)
            {
                _context.Articles.Remove(articles);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexArticles));
        }

        private bool ArticlesExists(int id)
        {
            return _context.Articles.Any(e => e.IdArticle == id);
        }
        public async Task<IActionResult> IndexBaits()
        {
            return View(await _context.Baits.ToListAsync());
        }

        // GET: Baits/Details/5
        public async Task<IActionResult> DetailsBaits(int? id)
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
        public IActionResult CreateBaits()
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
        public async Task<IActionResult> EditBaits(int? id)
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
        public async Task<IActionResult> EditBaits(int id, [Bind("Id,BaitsHtml,Date,PhotoPreview,Title,Announce")] Bait bait)
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
        public async Task<IActionResult> DeleteBaits(int? id)
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
        [HttpPost, ActionName("DeleteBaits")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedBaits(int id)
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
        // GET: KindOfFish
        public async Task<IActionResult> IndexFish()
        {
            return View(await _context.KindOfFishes.ToListAsync());
        }

        // GET: KindOfFish/Details/5
        public async Task<IActionResult> DetailsFish(int? id)
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
        public IActionResult CreateFish()
        {
            return View();
        }

        // POST: KindOfFish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFish([Bind("Id,KindOfFishHtml,Date,PhotoPreview,Title,Announce")] KindOfFish kindOfFish)
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
        public async Task<IActionResult> EditFish(int? id)
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
        public async Task<IActionResult> EditFish(int id, [Bind("Id,KindOfFishHtml,Date,PhotoPreview,Title,Announce")] KindOfFish kindOfFish)
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
        public async Task<IActionResult> DeleteFish(int? id)
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
        [HttpPost, ActionName("DeleteFish")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedFish(int id)
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
        public async Task<IActionResult> IndexLures()
        {
            return View(await _context.Lures.ToListAsync());
        }

        // GET: Lures/Details/5
        public async Task<IActionResult> DetailsLures(int? id)
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
        public IActionResult CreateLures()
        {
            return View();
        }

        // POST: Lures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLures([Bind("Id,LuresHtml,Date,Title,Announce,PhotoPreview")] Lure lure)
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
        public async Task<IActionResult> EditLures(int? id)
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
        public async Task<IActionResult> EditLures(int id, [Bind("Id,LuresHtml,Date,Title,Announce,PhotoPreview")] Lure lure)
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
        public async Task<IActionResult> DeleteLures(int? id)
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
        [HttpPost, ActionName("DeleteLures")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedLures(int id)
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