namespace _3TeamProject.Areas.Sightseeings.Data
{
    public class SightMessageViewModel
    {
        public int MessageBoardId { get; set; }
        public string MemberName { get; set; }
        public string? SightseeingMessageContent { get; set; }
        public DateTime MessageCreatedTime { get; set; }
    }
}
