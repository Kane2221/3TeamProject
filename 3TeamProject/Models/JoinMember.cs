using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class JoinMember
    {
        public int JoinId { get; set; }
        public int ActivitiesId { get; set; }
        public int MemberId { get; set; }

        public virtual SocialActivity Activities { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
