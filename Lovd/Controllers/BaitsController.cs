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
        // GET: Baits

    }
}
