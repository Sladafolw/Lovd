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
    public class CommentsController : Controller
    {
        private readonly LoveContext _context;

        public CommentsController(LoveContext context)
        {
            _context = context;
        }

        // GET: Comments
       
    }
}
