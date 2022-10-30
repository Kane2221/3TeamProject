namespace _3TeamProject.Areas.Members.Data
{
    public class GetActRecord
    {
        public int ActivityId { get; set; }
        public string ActivitiesName { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
