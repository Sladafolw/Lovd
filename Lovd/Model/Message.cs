using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class Message
    {
        public string? Text { get; set; }
        public byte[]? Video { get; set; }
        public byte[]? Photo { get; set; }
        public long MessagesId { get; set; }
        public int TopicId { get; set; }
        public DateTime DateForumMessages { get; set; }
        public long? ReplyMessagesId { get; set; }
        public DateTime? EditDate { get; set; }

        public virtual TopicForum Topic { get; set; } = null!;
    }
}
