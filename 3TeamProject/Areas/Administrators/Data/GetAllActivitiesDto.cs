namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetAllActivitiesDto
    {
        public int ActivityId { get; set; }
        public string MemberName { get; set; } = null!;
        public string ActivitiesName { get; set; } = null!;
        public string ActivitiesAddress { get; set; } = null!;
        public string? CreatedTime { get; set; }
        public string? EndTime { get; set; }
        public int? LimitCount { get; set; }
        public int JoinCount { get; set; }
    }
}
