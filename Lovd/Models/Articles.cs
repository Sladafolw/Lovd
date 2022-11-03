using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Articles
    {
        public int IdNews { get; set; }
        public string ArticleHtml { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public DateTime DateNews { get; set; }
        public string Title { get; set; } = null!;
        public string Announce { get; set; } = null!;
        public byte[] PhotoPreview { get; set; } = null!;

        public virtual AspNetUser User { get; set; } = null!;
    }
}
