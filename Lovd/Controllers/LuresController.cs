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
    public class LuresController : Controller
    {
        private readonly LoveContext _context;

        public LuresController(LoveContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MainPage(int page)
        {
            if (Math.Ceiling((double)LuresCount() / 8) >= page)
            {
                ViewBag.Count = LuresCount();
                if (page == 0)
                {
                    var _Lures = (from lures in _context.Lures
                                    where (lures.PhotoPreview != null)
                                    select new
                                    {
                                        IdArticle = lures.Id,
                                        DateNews = lures.Date,
                                        Title = lures.Title,
                                        Announce = lures.Announce,
                                    
                                        PhotoPreview = Convert.ToBase64String(lures.PhotoPreview)
                                    }).Take(8).AsEnumerable().ToList();
                    return View(_Lures);
                }
                else
                {
                    var lures = (from _lures in _context.Lures
                                   where (_lures.PhotoPreview != null)
                                   select new
                                   {
                                       IdArticle = _lures.Id,
                                       DateNews = _lures.Date,
                                       Title = _lures.Title,
                                       Announce = _lures.Announce,
                                      
                                       PhotoPreview = Convert.ToBase64String(_lures.PhotoPreview)
                                   }).Skip((page) * 8).Take(8).AsEnumerable().ToList();
                    return View(lures);
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DifferentsLures(int id)
        {
            if (LuresExsist(id))
            {
                var lure = _context.Lures
                .FirstOrDefault(n => n.Id == id);
           
                ViewBag.PageHtml = lure.LuresHtml;
                ViewBag.id = id.ToString();
              
          

                return View();
            }
            return RedirectToAction(nameof(MainPage));

        }
        public bool LuresExsist(int id)
        {
            return _context.Lures.Any(p => p.Id == id);
        }
        public int LuresCount()
        {
            return _context.Lures.Count();
        }
    }
}
