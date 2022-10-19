using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class TopicForum
    {
        public TopicForum()
        {
            Messages = new HashSet<Message>();
        }

        public string UserId { get; set; } = null!;
        public int TopicId { get; set; }
        public string TopicName { get; set; } = null!;

        public virtual ICollection<Message> Messages { get; set; }
    }
}
