using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Article
    {
        public Article()
        {
            Comments = new HashSet<Comment>();
            LikesWithDislikes = new HashSet<LikesWithDislike>();
        }

        public int IdArticle { get; set; }
        public string ArticleHtml { get; set; } = null!;
        public int? Likes { get; set; }
        public int? DisLikes { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime DateNews { get; set; }
        public string Title { get; set; } = null!;
        public string Announce { get; set; } = null!;
        public byte[]? PhotoPreview { get; set; }

        public virtual AspNetUser  User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikesWithDislike> LikesWithDislikes { get; set; }
    }
}
