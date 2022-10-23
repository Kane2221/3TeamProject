using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class SightseeingStatusCategory
    {
        public SightseeingStatusCategory()
        {
            SightseeingMessageBoards = new HashSet<SightseeingMessageBoard>();
        }

        public int SightseeingStatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<SightseeingMessageBoard> SightseeingMessageBoards { get; set; }
    }
}
