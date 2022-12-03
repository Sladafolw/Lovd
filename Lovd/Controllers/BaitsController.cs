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
    public class BaitsController : Controller
    {
        private readonly LoveContext _context;

        public BaitsController(LoveContext context)
        {
            _context = context;
        }
        public IActionResult Search(string name)
        {

            IEnumerable<dynamic> a = _context.Baits.Where(l => l.Title.StartsWith(name)).ToList();

            return View("MainPage", a);
        }
        public async Task<IActionResult> MainPage(int page)
        {
            if (Math.Ceiling((double)BaitsCount() / 9) >= page)
            {
                ViewBag.Count = BaitsCount();
                if (page == 0)
                {
                    var __baits = (from baits in _context.Baits
                                  where (baits.PhotoPreview != null)
                                  select new
                                  {
                                      Id = baits.Id,
                                      Date = baits.Date,
                                      Title = baits.Title,
                                      Announce = baits.Announce,
                                      PhotoPreview = baits.PhotoPreview
                                  }).Take(9).AsEnumerable().ToList();
                    return View(__baits);
                }
                else
                {
                    var baits = (from _baits in _context.Baits
                                 where (_baits.PhotoPreview != null)
                                 select new
                                 {
                                     Id = _baits.Id,
                                     DateNews = _baits.Date,
                                     Title = _baits.Title,
                                     PhotoPreview = _baits.PhotoPreview
                                 }).Skip((page) * 9).Take(9).AsEnumerable().ToList();
                    return View(baits);
                }
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DifferentsBaits(int id)
        {
            if (BaitsExsist(id))
            {
                var baits = _context.Baits
                .FirstOrDefault(n => n.Id == id);

                ViewBag.PageHtml = baits.BaitsHtml;
                ViewBag.id = id.ToString();

                return View();
            }
            return RedirectToAction(nameof(MainPage));

        }
    
        public bool BaitsExsist(int id)
        {
            return _context.Baits.Any(p => p.Id == id);
        }
        public int BaitsCount()
        {
            return _context.Baits.Count();
        }
    }

}
