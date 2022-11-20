using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lovd.Models;
using Lovd.Data;

using Lovd.ModelsView;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity;

namespace Lovd.Controllers
{
    public class ArticleController : Controller
    {
        private readonly LoveContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public ArticleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, LoveContext context)
        {
            this.roleManager = roleManager;
            _context = context;
            _userManager = userManager;
        }

        // GET: News
        public async Task<IActionResult> MainPage(int page)
        {
            if (Math.Ceiling((double)ArticlesCount() / 8) >= page)
            {
                ViewBag.Count = ArticlesCount();
                if (page == 0)
                {
                    var _Article = (from article in _context.Articles
                                    where (article.PhotoPreview != null)
                                    select new
                                    {
                                        IdArticle = article.IdArticle,
                                        DateNews = article.DateNews,
                                        Title = article.Title,
                                        Announce = article.Announce,
                                        DisLikes = article.DisLikes ?? 0,
                                        Likes = article.Likes ?? 0,
                                        PhotoPreview = Convert.ToBase64String(article.PhotoPreview)
                                    }).Take(8).AsEnumerable().ToList();
                    return View(_Article);
                }
                else
                {
                    var Article = (from article in _context.Articles
                                   where (article.PhotoPreview != null)
                                   select new
                                   {
                                       IdArticle = article.IdArticle,
                                       DateNews = article.DateNews,
                                       Title = article.Title,
                                       Announce = article.Announce,
                                       DisLikes = article.DisLikes ?? 0,
                                       Likes = article.Likes ?? 0,
                                       PhotoPreview = Convert.ToBase64String(article.PhotoPreview)
                                   }).Skip((page) * 8).Take(8).AsEnumerable().ToList();
                    return View(Article);
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DifferentsArticles(int id)
        {
            if (ArticlesExsist(id))
            {
                var articles = _context.Articles
                .FirstOrDefault(n => n.IdArticle == id);
                ViewBag.Likes= articles?.Likes ?? 0;
                ViewBag.DisLikes = articles?.DisLikes ?? 0;
                ViewBag.PageHtml = articles.ArticleHtml;
                ViewBag.id= id.ToString();
                ArticleComments articleComments = new ArticleComments();
                List<Comment> comment = new();
                var commentsAll = _context.Comments.Include(n => n.User).Where(n => n.IdArticle == id);
                ;
                foreach (var item in commentsAll)
                {
                    comment.Add(item);
         
                }
                articleComments.comments = comment;

                return View(articleComments);
            }
            return RedirectToAction(nameof(MainPage));

        }

        public async Task<IActionResult> CommentsCreatePartial([Bind("comment")] ModelsView.ArticleComments model)
        {
            if (!ModelState.IsValid)
            {
                Comment comment = new();
                comment.Text = model.comment;
                comment.UserId = GetCurrentUserId();
                comment.CreatedDate = DateTime.Now;
                _context.Add(comment);
                await _context.SaveChangesAsync();

            }

           
            return PartialView();
        }
        public bool ArticlesExsist(int id)
        {
            return _context.Articles.Any(p => p.IdArticle == id);
        }
        public int ArticlesCount()
        {
            return _context.Articles.Count();
        }
        private  string GetCurrentUserId() =>  _userManager.GetUserId(HttpContext.User);
    }
}
