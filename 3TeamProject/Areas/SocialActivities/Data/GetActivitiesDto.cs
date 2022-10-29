using _3TeamProject.Models;

namespace _3TeamProject.Areas.SocialActivities.Data
{
    public class GetActivitiesDto
    {
        public int ActivityId { get; set; }
        public string MemberName { get; set; } = null!;
        public string ActivitiesName { get; set; } = null!;
        public string ActivitiesAddress { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? LimitCount { get; set; }
        public int JoinCount { get; set; }

    }
}
