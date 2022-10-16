using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class User
    {
        public User()
        {
            News = new HashSet<News>();
            TopicForums = new HashSet<TopicForum>();
        }

        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool? Online { get; set; }
        public DateTime LastOnline { get; set; }
        public string? ReturnUrl { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<TopicForum> TopicForums { get; set; }
    }
}
