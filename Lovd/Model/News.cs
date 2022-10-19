using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class News
    {
        public News()
        {
            Comments = new HashSet<Comment>();
            LikesWithDislikes = new HashSet<LikesWithDislike>();
        }

        public int IdNews { get; set; }
        public string NewsHtml { get; set; } = null!;
        public int? Likes { get; set; }
        public int? DisLikes { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime DateNews { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<LikesWithDislike> LikesWithDislikes { get; set; }
    }
}
