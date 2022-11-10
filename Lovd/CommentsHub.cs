using Lovd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Build.Graph;

namespace _3psp
{
    [Route("/chat")]
    public class CommentsHub : Hub
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly LoveContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public CommentsHub(IHttpContextAccessor httpContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, LoveContext context)
        {
            _httpContext = httpContext;
            this.roleManager = roleManager;
            _context = context;
            _userManager = userManager;
            var allArticles = _context.Articles;
            foreach (var article in allArticles)
            {
                groupNameList.Add(article.IdArticle.ToString());
            }
        }

        List<string> groupNameList = new List<string>();
        //public async Task Enter(string username)
        //{
        //    if (String.IsNullOrEmpty(username))
        //    {
        //        await Clients.Caller.SendAsync("Notify", "Для входа в чат введите логин");
        //    }
        //    else
        //    {
        //        var a = _userManager.GetUserName(_httpContext.HttpContext.User);
        //        await Groups.AddToGroupAsync(Context.ConnectionId, groupname);
        //        await Clients.Group(groupname).SendAsync("Notify", $"{a} вошел в чат");
        //    }
        //}
        string group;
        public bool GroupNameExsist(string group)
        {
            if (group != null && groupNameList != null)
            {
                foreach (var a in groupNameList)
                {
                    if (a == group)
                    {
                        this.group = group;
                        return true;
                    }
                    else return false;

                }
            }
            return false;
        }
       
    
        public async Task OnPageLoad(string group)
        {if(GroupNameExsist(group))
            await Groups.AddToGroupAsync(Context.ConnectionId, group);

        }
        public async Task Send(string message, string group)
        {

            await Clients.Group(group).SendAsync("Receive", message, GetName());
        }
        public string GetName() => _userManager.GetUserName(_httpContext.HttpContext.User);
    }
}
