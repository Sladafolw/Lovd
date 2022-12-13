using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Comment
    {
        public int IdArticle { get; set; }
        public string Text { get; set; } = null!;
        public int IdComments { get; set; }
        public int? ReplyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditDate { get; set; }
        public string UserId { get; set; } = null!;

        public virtual Article ?IdArticleNavigation { get; set; } = null!;
        public virtual IdentityUser? User { get; set; } = null!;
    }
}
