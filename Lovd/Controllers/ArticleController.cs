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
        public async Task<IActionResult> MainPage()
        { var Article= (from article in _context.Articles
                     where(article.PhotoPreview!=null)
                     select new{ DateNews = article.DateNews,Title = article.Title,Announce = article.Announce,DisLikes = article.DisLikes??0,Likes = article.Likes ?? 0 /*, PhotoPreview= Convert.ToBase64String(news.PhotoPreview)*/
                     }).AsEnumerable().ToList();
            return View(Article);
        }

      
    }
}
