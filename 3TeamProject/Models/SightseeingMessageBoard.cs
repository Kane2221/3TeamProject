using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class SightseeingMessageBoard
    {
        public int MessageBoardId { get; set; }
        public int SightseeingId { get; set; }
        public int MemberId { get; set; }
        public string? SightseeingMessageContent { get; set; }
        public DateTime MessageCreatedTime { get; set; }
        public int SightseeingMessageState { get; set; }

        public virtual Sightseeing Sightseeing { get; set; } = null!;
    }
}
