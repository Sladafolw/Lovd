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
    public class KindOfFishController : Controller
    {
        private readonly LoveContext _context;

        public KindOfFishController(LoveContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> MainPage(int page)
        {
            if (Math.Ceiling((double)FishCount() / 8) >= page)
            {
                ViewBag.Count = FishCount();
                if (page == 0)
                {
                    var _fish = (from fish in _context.KindOfFishes
                                  where (fish.PhotoPreview != null)
                                  select new
                                  {
                                      IdArticle = fish.Id,
                                      DateNews = fish.Date,
                                      Title = fish.Title,
                                      Announce = fish.Announce,

                                      PhotoPreview = Convert.ToBase64String(fish.PhotoPreview)
                                  }).Take(8).AsEnumerable().ToList();
                    return View(_fish);
                }
                else
                {
                    var fish = (from _fish in _context.KindOfFishes
                                 where (_fish.PhotoPreview != null)
                                 select new
                                 {
                                     IdArticle = _fish.Id,
                                     DateNews = _fish.Date,
                                     Title = _fish.Title,
                                     Announce = _fish.Announce,

                                     PhotoPreview = Convert.ToBase64String(_fish.PhotoPreview)
                                 }).Skip((page) * 8).Take(8).AsEnumerable().ToList();
                    return View(fish);
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DifferentsFish(int id)
        {
            if (FishExsist(id))
            {
                var _fish = _context.KindOfFishes
                .FirstOrDefault(n => n.Id == id);

                ViewBag.PageHtml = _fish.KindOfFishHtml;
                ViewBag.id = id.ToString();



                return View();
            }
            return RedirectToAction(nameof(MainPage));

        }
        public bool     FishExsist(int id)
        {
            return _context.KindOfFishes.Any(p => p.Id == id);
        }
        public int FishCount()
        {
            return _context.Lures.Count();
        }

    }
}
