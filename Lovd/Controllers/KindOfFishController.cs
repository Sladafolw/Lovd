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
            if (Math.Ceiling((double)FishCount() / 9) >= page)
            {
                ViewBag.Count = FishCount();
                if (page == 0)
                {
                    var _fish = (from fish in _context.KindOfFishes
                                  where (fish.PhotoPreview != null)
                                  select new
                                  {
                                      Id = fish.Id,
                                      Date = fish.Date,
                                      Title = fish.Title,
                                      Announce = fish.Announce,

                                      PhotoPreview = fish.PhotoPreview
                                  }).Take(9).AsEnumerable().ToList();
                    return View(_fish);
                }
                else
                {
                    var fish = (from _fish in _context.KindOfFishes
                                 where (_fish.PhotoPreview != null)
                                 select new
                                 {
                                     Id = _fish.Id,
                                     Date = _fish.Date,
                                     Title = _fish.Title,
                                     Announce = _fish.Announce,

                                     PhotoPreview = _fish.PhotoPreview
                                 }).Skip((page) * 9).Take(9).AsEnumerable().ToList();
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
        public IActionResult Search(string name)
        {

            IEnumerable<dynamic> Content =  _context.KindOfFishes.Where(l => l.Title.StartsWith(name)).ToList();
        
            return View("MainPage", Content);
        }
        public int FishCount()
        {
            return _context.Lures.Count();
        }

    }
}
