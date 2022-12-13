using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Pond
    {
        public int Id { get; set; }
        public string PondsHtml { get; set; } = null!;
        public DateTime Date { get; set; }
        public byte[] PhotoPreview { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Announce { get; set; } = null!;
        public string IdUser { get; set; } = null!;

        public virtual IdentityUser IdUserNavigation { get; set; } = null!;
    }
}
