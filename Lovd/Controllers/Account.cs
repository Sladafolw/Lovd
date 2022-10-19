using Microsoft.AspNetCore.Mvc;

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
