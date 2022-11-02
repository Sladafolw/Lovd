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
    public class NewsController : Controller
    {
        private readonly LoveContext _context;

        public NewsController(LoveContext context)
        {
            this._context = context;
        }

        // GET: News
        public async Task<IActionResult> MainPage()
        { var News= (from news in _context.News
                     select new{ DateNews=news.DateNews,Title=news.Title,Announce= news.Announce,DisLikes=news.DisLikes??0,Likes=news.Likes}).AsEnumerable().ToList();
            return View(News);
        }

      
    }
}
