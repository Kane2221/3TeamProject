namespace _3TeamProject.Areas.Sightseeings.Data
{
    public class SightMessageDto
    {
        public string UserRole { get; set; } = null!;
        public int MessageBoardId { get; set; }
        public string Account { get; set; } = null!;
        public string? SightseeingMessageContent { get; set; }
        public string? MessageCreatedTime { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
