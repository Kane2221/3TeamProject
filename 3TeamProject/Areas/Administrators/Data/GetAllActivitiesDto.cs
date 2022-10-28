namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetAllActivitiesDto
    {
        public int ActivityId { get; set; }
        public string MemberName { get; set; } = null!;
        public string ActivitiesName { get; set; } = null!;
        public string? ActivitiesContent { get; set; }
        public string ActivitiesAddress { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? LimitCount { get; set; }
        public int JoinCount { get; set; }
        public DateTime ActitiesStartDate { get; set; }
        public DateTime ActitiesFinishDate { get; set; }
    }
}
