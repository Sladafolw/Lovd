using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lovd.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
