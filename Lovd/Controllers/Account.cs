using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lovd.Controllers
{
    public class Account : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
