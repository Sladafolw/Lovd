using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class KindOfFish
    {
        public int Id { get; set; }
        public string KindOfFishHtml { get; set; } = null!;
        public DateTime Date { get; set; }
        public byte[]? PhotoPreview { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Announce { get; set; } = null!;
    }
}
