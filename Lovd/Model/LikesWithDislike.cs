using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class LikesWithDislike
    {
        public bool? Dislike { get; set; }
        public bool? Like { get; set; }
        public int IdNews { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }

        public virtual News IdNewsNavigation { get; set; } = null!;
    }
}
