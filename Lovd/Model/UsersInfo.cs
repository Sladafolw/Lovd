using System;
using System.Collections.Generic;

namespace Lovd.Models
{
    public partial class UsersInfo
    {
        public string UserId { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public bool? Online { get; set; }
        public DateTime LastOnline { get; set; }
    }
}
