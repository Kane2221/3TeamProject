using _3TeamProject.Models;

namespace _3TeamProject.Areas.SocialActivities.Data
{
    public class GetActMsgBoardDto
    {
        public int ActivitiesMessageId { get; set; }
        public string? ActivitiesMessageContent { get; set; }
        public DateTime ActivitiesCreatedDate { get; set; }
        public string Account { get; set; } = null!;

    }
}
