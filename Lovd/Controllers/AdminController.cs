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
        public async Task<IActionResult> CreateArticles([Bind("IdArticle,NewsHtml,Likes,DisLikes,UserId,DateNews,Title,Announce")] Article article, IFormFile PhotoPreview)
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", article.UserId);
            return View(article);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditArticles(int id, [Bind("IdArticle,NewsHtml,Likes,DisLikes,UserId,DateNews,Title,Announce")] Article articles)
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articles == null)
            {
                return Problem("Entity set 'LoveContext.News'  is null.");
            }
            var articles = await _context.Articles.FindAsync(id);
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

    }

}