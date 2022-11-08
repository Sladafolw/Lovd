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
            {
                ViewBag.Count = ArticlesCount();
                if (page == 1)
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
                               }).Skip(page * 8).Take(8).AsEnumerable().ToList();
                return View(Article);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DifferentsArticles(int IdArticle)
        {
            if (ArticlesExsist(IdArticle))
            {
                var articles = _context.Articles
                .FirstOrDefault(n => n.IdArticle == IdArticle);
                 ViewBag.PageHtml = articles.ArticleHtml; 
                ArticleComments articleComments = new ArticleComments();
                List <Comment> comment = new ();
                var commentsAll = _context.Comments.Where(n => n.IdArticle == IdArticle);
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
        public  JsonResult CreateComments(int IdArticle)
        {
            if (ModelState.IsValid)
            {
                //var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
                //if (result.Succeeded)
                //{
                //     //return RedirectToLocal(returnUrl);
                //}

                ModelState.AddModelError("", "Identifiant ou mot de passe invalide");
                return Json("error-model-wrong");
            }

            // If we got this far, something failed, redisplay form
            return Json("error-mode-not-valid");
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
