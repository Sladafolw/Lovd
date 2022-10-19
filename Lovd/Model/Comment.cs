using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Comment
    {
        public int IdNews { get; set; }
        public string Text { get; set; } = null!;
        public int IdComments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? EditDate { get; set; }

        public virtual News IdNewsNavigation { get; set; } = null!;
    }
}
