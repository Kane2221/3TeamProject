using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class ActivitiesMessageBoard
    {
        public int ActivitiesMessageId { get; set; }
        public int ActivitiesId { get; set; }
        public string? ActivitiesMessageContent { get; set; }
        public DateTime ActivitiesCreatedDate { get; set; }
        public int MemberId { get; set; }
        public string ActivitiesMessageState { get; set; } = null!;

        public virtual SocialActivity Activities { get; set; } = null!;
        public virtual Member Member { get; set; } = null!;
    }
}
