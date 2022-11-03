using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            Articles = new HashSet<Article>();
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            Comments = new HashSet<Comment>();
            News = new HashSet<News>();
            Ponds = new HashSet<Pond>();
            TopicForums = new HashSet<TopicForum>();
            Roles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public virtual UsersInfo? UsersInfo { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Pond> Ponds { get; set; }
        public virtual ICollection<TopicForum> TopicForums { get; set; }

        public virtual ICollection<AspNetRole> Roles { get; set; }
    }
}
