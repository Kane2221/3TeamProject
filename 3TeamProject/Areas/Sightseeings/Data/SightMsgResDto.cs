namespace _3TeamProject.Areas.Sightseeings.Data
{
	public class SightMsgResDto
	{
        public int SightseeingId { get; set; }
        public int MessageBoardId { get; set; }
        public string Account { get; set; } = null!;
        public string? SightseeingMessageContent { get; set; }
        public DateTime MessageCreatedTime { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
