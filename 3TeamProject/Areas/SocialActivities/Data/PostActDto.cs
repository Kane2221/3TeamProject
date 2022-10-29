namespace _3TeamProject.Areas.SocialActivities.Data
{
    public class PostActDto
    {
        public string ActivitiesName { get; set; } = null!;
        public string? ActivitiesContent { get; set; }
        public string ActivitiesAddress { get; set; } = null!;
        public DateTime EndTime { get; set; }
        public int? LimitCount { get; set; }
        public DateTime ActitiesStartDate { get; set; }
        public DateTime ActitiesFinishDate { get; set; }

    }
}
