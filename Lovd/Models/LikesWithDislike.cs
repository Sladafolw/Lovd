using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class LikesWithDislike
    {
        public bool? Dislike { get; set; }
        public bool? Like { get; set; }
        public int IdArticle { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; } = null!;

        public virtual Article IdArticleNavigation { get; set; } = null!;
    }
}
