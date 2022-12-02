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
            if (Math.Ceiling((double)LuresCount() / 9) >= page)
            {
                ViewBag.Count = LuresCount();
                if (page == 0)
                {
                    var _Lures = (from lures in _context.Lures
                                    where (lures.PhotoPreview != null)
                                    select new
                                    {
                                        Id = lures.Id,
                                        Date = lures.Date,
                                        Title = lures.Title,
                                        Announce = lures.Announce,
                                    
                                        PhotoPreview = lures.PhotoPreview
                                    }).Take(9).AsEnumerable().ToList();
                    return View(_Lures);
                }
                else
                {
                    var lures = (from _lures in _context.Lures
                                   where (_lures.PhotoPreview != null)
                                   select new
                                   {
                                       Id= _lures.Id,
                                       DateNews = _lures.Date,
                                       Title = _lures.Title,
                                       Announce = _lures.Announce,
                                      
                                       PhotoPreview = _lures.PhotoPreview
                                   }).Skip((page) * 9).Take(9).AsEnumerable().ToList();
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
        public IActionResult Search(string name)
        {

            IEnumerable<dynamic> a = _context.Lures.Where(l => l.Title.StartsWith(name)).ToList();


            return View("MainPage",a);
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
