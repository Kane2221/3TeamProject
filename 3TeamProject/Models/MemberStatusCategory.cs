using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class MemberStatusCategory
    {
        public MemberStatusCategory()
        {
            Members = new HashSet<Member>();
        }

        public int MemberStatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Member> Members { get; set; }
    }
}
