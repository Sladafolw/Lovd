
using Lovd.Data;
using Lovd.Models;
using Lovd.ModelsView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;

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

        [HttpPost]
        public async Task<ActionResult>  UserFind(string name)
        {
            PartialView("AllUserWithRoles", "namw");
            return View("SetUserRole");
        }
        [HttpPost]
        public async Task<ActionResult> SetUserRole(string name, string role)
        {
            IdentityUser user = await _userManager?.FindByNameAsync(name);
            IdentityRole _role = await roleManager?.FindByNameAsync(role);
            if (user != null && _role != null)
            {
              await _userManager.AddToRoleAsync(user,role);
              await _context.SaveChangesAsync();
            }
           return RedirectToAction(nameof(Admin));
        
        }
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
            if (ModelState.IsValid && PhotoPreview != null)
            {

                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(PhotoPreview.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)PhotoPreview.Length);
                }
                // установка массива байтов
                article.PhotoPreview = imageData;
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

            var article = (from _article in _context.Articles
                           where ( _article.IdArticle == id)
                           select new
                           {
                               IdArticle=_article.IdArticle,
                               UserId = _article.UserId,
                               DateNews = _article.DateNews,
                               Title = _article.Title,
                               Announce = _article.Announce,
                               Likes = _article.Likes,
                               DisLikes = _article.DisLikes,
                               ArticleHtml = _article.ArticleHtml,
                               PhotoPreview = _article.PhotoPreview
                           }).AsEnumerable().ToList();
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
        public async Task<IActionResult> EditArticles(int id, [Bind("IdArticle,ArticleHtml,Likes,DisLikes,UserId,DateNews,Title,Photo,PhotoPreview,Announce")] ArticleView articles)
        {
            
            if (articles.Photo == null)
            {
                var photo = (from fish in _context.Articles
                             where (fish.PhotoPreview != null && fish.IdArticle == articles.IdArticle)
                             select new
                             {
                                 photoB = fish.PhotoPreview
                             });
                foreach (var a in photo)
                {
                    articles.PhotoPreview = a.photoB;

                }


            }
            else
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(articles.Photo.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)articles.Photo.Length);
                }
                articles.PhotoPreview = imageData;
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
        public IActionResult SetUserRole()
        {
            return View();
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
        public async Task<IActionResult> CreateBaits([Bind("Id,BaitsHtml,Date,Title,Announce")]  Bait bait, IFormFile PhotoPreview)
        {
            if (ModelState.IsValid && PhotoPreview != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(PhotoPreview.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)PhotoPreview.Length);
                }
                // установка массива байтов
                bait.PhotoPreview = imageData;


                _context.Add(bait);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexBaits));

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

            var bait = (from _Baits in _context.Baits
                        where (_Baits.PhotoPreview != null && _Baits.Id == id)
                        select new
                        {
                            Id = _Baits.Id,
                            Date = _Baits.Date,
                            Title = _Baits.Title,
                            Announce = _Baits.Announce,
                            BaitsHtml = _Baits.BaitsHtml,
                            PhotoPreview = _Baits.PhotoPreview
                        }).AsEnumerable().ToList();
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
        public async Task<IActionResult> EditBaits(int id, [Bind("Id,BaitsHtml,Date,Photo,Title,Announce")] BaitView bait)
        {
            if (id != bait.Id)
            {
                return NotFound();
            }
            if (bait.Photo == null)
            {
                var photo = (from baits in _context.Baits
                             where (baits.PhotoPreview != null && baits.Id == id)
                             select new
                             {
                                 photoB = baits.PhotoPreview
                             });
                foreach (var a in photo)
                {
                    bait.PhotoPreview = a.photoB;


                }


            }
            else
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(bait.Photo.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)bait.Photo.Length);
                }
                bait.PhotoPreview = imageData;
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
                return RedirectToAction(nameof(IndexBaits));
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
            return RedirectToAction(nameof(IndexBaits));
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
        public async Task<IActionResult> CreateFish([Bind("Id,KindOfFishHtml,Date,PhotoPreview,Title,Announce")] KindOfFish kindOfFish, IFormFile PhotoPreview)
        {
            if (ModelState.IsValid && PhotoPreview != null)
            {

                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(PhotoPreview.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)PhotoPreview.Length);
                }
                // установка массива байтов
                kindOfFish.PhotoPreview = imageData;
                _context.Add(kindOfFish);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexFish));

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

            var kindOfFish =  (from fish in _context.KindOfFishes
                                    where (fish.PhotoPreview != null && fish.Id==id)
                                    select new
                                    {
                                        Id = fish.Id,
                                        Date = fish.Date,
                                        Title = fish.Title,
                                        Announce = fish.Announce,
                                        KindOfFishHtml= fish.KindOfFishHtml,

                                        PhotoPreview = fish.PhotoPreview
                                    }).AsEnumerable().ToList();
          
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
        public async Task<IActionResult> EditFish(int id, [Bind("Id,KindOfFishHtml,Date,Title,Announce,Photo")] KindView kindOfFish)
        {
            if (id != kindOfFish.Id)
            {
                return NotFound();
            }
         
            if (kindOfFish.Photo == null)
            {
                var photo = (from fish in _context.KindOfFishes
                             where (fish.PhotoPreview != null && fish.Id == id)
                             select new
                             {
                                 photoB = fish.PhotoPreview
                             });
                foreach (var a in photo)
                {
                    kindOfFish.PhotoPreview = a.photoB;
                   

                }


            }
            else
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(kindOfFish.Photo.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)kindOfFish.Photo.Length);
                }
                kindOfFish.PhotoPreview=imageData;

              

            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.KindOfFishes.Update(kindOfFish);
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
                return RedirectToAction(nameof(IndexFish));
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
            return RedirectToAction(nameof(IndexFish));
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
        public async Task<IActionResult> CreateLures([Bind("Id,LuresHtml,Date,Title,Announce")] Lure lure, IFormFile PhotoPreview)
        {
            if (ModelState.IsValid && PhotoPreview != null)
            {

                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(PhotoPreview.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)PhotoPreview.Length);
                }
                lure.PhotoPreview = imageData;
                _context.Add(lure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexLures));
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

            var lure =
              (from Lure in _context.Lures
                              where (Lure.PhotoPreview != null && Lure.Id == id)
                              select new
                              {
                                  Id = Lure.Id,
                                  Date = Lure.Date,
                                  Title = Lure.Title,
                                  Announce = Lure.Announce,
                                  LuresHtml = Lure.LuresHtml,

                                  PhotoPreview = Lure.PhotoPreview
                              }).AsEnumerable().ToList();
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
        public async Task<IActionResult> EditLures(int id, [Bind("Id,LuresHtml,Date,Title,Announce,Photo")] LuresView lure)
        {
            if (id != lure.Id)
            {
                return NotFound();
            }
            if (lure.Photo == null)
            {
                var photo = (from fish in _context.Lures
                             where (fish.PhotoPreview != null && fish.Id == id)
                             select new
                             {
                                 photoB = fish.PhotoPreview
                             });
                foreach (var a in photo)
                {
                    lure.PhotoPreview = a.photoB;


                }


            }
            else
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(lure.Photo.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)lure.Photo.Length);
                }
                lure.PhotoPreview = imageData;
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
                return RedirectToAction(nameof(IndexLures));
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
            return RedirectToAction(nameof(IndexLures));
        }

        private bool LureExists(int id)
        {
            return _context.Lures.Any(e => e.Id == id);
        }
    }

}