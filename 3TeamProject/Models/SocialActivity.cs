using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class SocialActivity
    {
        public SocialActivity()
        {
            ActivitiesMessageBoards = new HashSet<ActivitiesMessageBoard>();
        }

        public int ActivitiesId { get; set; }
        public int MemberId { get; set; }
        public string ActivitiesName { get; set; } = null!;
        public string? ActivitiesContent { get; set; }
        public string ActivitiesAddress { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? LimitCount { get; set; }
        public int JoinCount { get; set; }
        public DateTime ActitiesStartDate { get; set; }
        public DateTime ActitiesFinishDate { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual ICollection<ActivitiesMessageBoard> ActivitiesMessageBoards { get; set; }
    }
}
