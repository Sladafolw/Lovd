using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lovd.Models;
using Lovd.Data;

namespace Lovd.Controllers
{
    public class ArticleController : Controller
    {
        private readonly LoveContext _context;

        public ArticleController(LoveContext context)
        {
            this._context = context;
        }

        // GET: News
        public async Task<IActionResult> MainPage(int page)
        {
            if ((double)ArticlesCount() / 8 >= page - 1)
            {ViewBag.Count=ArticlesCount();
                if (page == 1) {
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
                               }).Skip(page*8).Take(8).AsEnumerable().ToList();
                return View(Article);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DifferentsArticles(int IdArticle)
        {

            if (ArticlesExsist(IdArticle))
            {
              
                    var Article1 = (from article in _context.Articles
                                    where (article.IdArticle == IdArticle)
                                    select new { PageHtml = article.ArticleHtml }).AsEnumerable().ToList();
                    return View(Article1);

                
            }

           return RedirectToAction(nameof(MainPage));
          
        }


        
        public bool ArticlesExsist(int id)
        {
            return _context.Articles.Any(p => p.IdArticle == id);
        }
        public int ArticlesCount()
        {
            return _context.Articles.Count();
        }

    }
}
