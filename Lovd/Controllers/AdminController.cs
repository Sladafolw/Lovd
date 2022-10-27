using Lovd.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lovd.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager <IdentityRole> roleManager;
        public AdminController(RoleManager<IdentityRole> roleManager) 
        {this.roleManager = roleManager;
        }
      
        // GET: Admin/Details/5
        public ActionResult Index(int id)
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> Create()
        { return View(); }

            // GET: Admin/Create
            [HttpPost]
        public async Task <ActionResult> Create(string roleName)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) 
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return View();
        }

        // POST: Admin/Create
       
    }

}