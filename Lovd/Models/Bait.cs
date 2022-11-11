using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Bait
    {
        public int Id { get; set; }
        public string BaitsHtml { get; set; } = null!;
        public DateTime Date { get; set; }
        public byte[]? PhotoPreview { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Announce { get; set; } = null!;
    }
}
