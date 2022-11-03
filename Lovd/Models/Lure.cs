using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Lure
    {
        public int Id { get; set; }
        public string LuresHtml { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Title { get; set; } = null!;
        public string Announce { get; set; } = null!;
        public byte[] PhotoPreview { get; set; } = null!;
    }
}
