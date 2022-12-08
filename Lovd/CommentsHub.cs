﻿using Lovd.Models;
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
       
        public bool GroupNameExsist(string group)
        {
            if (group != null && groupNameList != null)
            {
                foreach (var a in groupNameList)
                {
                    if (a == group)
                    {
                        
                        return true;
                    }
                   

                }
            }
            return false;
        }
        public async Task RefreshLikesArticle(int articleId)
        {
           string group = articleId.ToString();
            var userLike = _context.LikesWithDislikes.Where(n => _userManager.GetUserId(_httpContext.HttpContext.User) == n.UserId && n.IdArticle == articleId).FirstOrDefault();
            if (GroupNameExsist(group) && userLike != null)
            {

                var alsm = from ass in _context.Articles
                         where (ass.IdArticle == articleId)
                         select new
                         {
                             DisLikes = ass.DisLikes ?? 0


                         };
                if (userLike.Dislike == true || userLike.Like == true)
                {
                    
                  _context.LikesWithDislikes.Remove(userLike);
                    await _context.SaveChangesAsync();
                   int counts=_context.Articles?.FirstOrDefault(n => n.IdArticle == articleId).Likes ?? 0;
                    var al = from ass in _context.Articles
                            where (ass.IdArticle == articleId)
                            select new
                            {
                                Likes = ass.Likes??0


                            };
                    await Clients.Group(group).SendAsync("Likes",al.FirstOrDefault().Likes,alsm.FirstOrDefault().DisLikes);
                    return;

                }
                else { userLike.Like = true; }
                _context.Update(userLike);
                await _context.SaveChangesAsync();
               
            }
            else if (userLike == null)
            {
                LikesWithDislike likesWithDislike = new LikesWithDislike();
                likesWithDislike.IdArticle = articleId;
                likesWithDislike.UserId = _userManager.GetUserId(_httpContext.HttpContext.User);
                likesWithDislike.Like = true;
               _context.Add(likesWithDislike);
                await _context.SaveChangesAsync();
            }
            var a = from ass in _context.Articles
                    where (ass.IdArticle == articleId)
                    select new
                    {
                        Likes = ass.Likes??0


                    }; var als = from ass in _context.Articles
                                 where (ass.IdArticle == articleId)
                                 select new
                                 {
                                     DisLikes = ass.DisLikes ?? 0


                                 };

            await Clients.Group(group).SendAsync("Likes", a.FirstOrDefault().Likes, als.FirstOrDefault().DisLikes) ;

        }


        public async Task RefreshDislikesArticle(int articleId)
            {
            string group = articleId.ToString();
            var userDisLike = _context.LikesWithDislikes.Where(n => _userManager.GetUserId(_httpContext.HttpContext.User) == n.UserId && n.IdArticle == articleId).FirstOrDefault();
            if (GroupNameExsist(group) && userDisLike != null)
            {

                var ab = from ass in _context.Articles
                        where (ass.IdArticle == articleId)
                        select new
                        {
                            Likes = ass.Likes ?? 0


                        };
                if (userDisLike.Dislike == true|| userDisLike.Like==true)
                {
                    _context.LikesWithDislikes.Remove(userDisLike);
                    await _context.SaveChangesAsync();
                    var al = from ass in _context.Articles
                             where (ass.IdArticle == articleId)
                             select new
                             {
                                 DisLikes = ass.DisLikes ?? 0


                             };
                    await Clients.Group(group).SendAsync("DisLikes",al.FirstOrDefault().DisLikes,ab.FirstOrDefault().Likes );
                    return;

                }
                else { userDisLike.Dislike = true; }
                _context.Update(userDisLike);
                await _context.SaveChangesAsync();
               
            }
            else if (userDisLike == null)
            {
                LikesWithDislike likesWithDislike = new LikesWithDislike();
                likesWithDislike.IdArticle = articleId;
                likesWithDislike.UserId = _userManager.GetUserId(_httpContext.HttpContext.User);
                likesWithDislike.Dislike = true;
                _context.Add(likesWithDislike);
                await _context.SaveChangesAsync();
            }
            var a = from ass in _context.Articles
                     where (ass.IdArticle == articleId)
                     select new
                     {
                         DisLikes = ass.DisLikes ?? 0


                     }; var abm = from ass in _context.Articles
                                 where (ass.IdArticle == articleId)
                                 select new
                                 {
                                     Likes = ass.Likes ?? 0


                                 };
            await Clients.Group(group).SendAsync("DisLikes", a.FirstOrDefault().DisLikes, abm.FirstOrDefault().Likes);

        }

        public async Task OnPageLoad(string group)
        {if(GroupNameExsist(group))
            await Groups.AddToGroupAsync(Context.ConnectionId, group);

        }
        public async Task Send(string message, string group)
        {
            if (GroupNameExsist(group)){
                Comment comment = new();
                comment.Text = message;
                comment.UserId = _userManager.GetUserId(_httpContext.HttpContext.User);
                comment.CreatedDate = DateTime.Now;
                comment.IdArticle = int.Parse(group);
                _context.Add(comment);
                await _context.SaveChangesAsync();
                await Clients.Group(group).SendAsync("Receive", message, GetName());
            } }
        public string CountLikes() => _userManager.GetUserName(_httpContext.HttpContext.User);

        public string GetName() => _userManager.GetUserName(_httpContext.HttpContext.User);
    }
}
