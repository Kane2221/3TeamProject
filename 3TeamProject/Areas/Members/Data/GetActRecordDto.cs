namespace _3TeamProject.Areas.Members.Data
{
    public class GetActRecordDto
    {
        public int ActivityId { get; set; }
        public string ActivitiesName { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
