using _3TeamProject.Models;

namespace _3TeamProject.Areas.SocialActivities.Data
{
    public class GetActivitiesDetailDto
    {
        public int ActivityId { get; set; }
        public string MemberName { get; set; }
        public string ActivitiesName { get; set; } = null!;
        public string? ActivitiesContent { get; set; }
        public string ActivitiesAddress { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public DateTime EndTime { get; set; }
        public int? LimitCount { get; set; }
        public int JoinCount { get; set; }
        public DateTime ActitiesStartDate { get; set; }
        public DateTime ActitiesFinishDate { get; set; }
        public string? ActivityPictureName { get; set; }
        public string? ActivityPicturePath { get; set; }

        public virtual IEnumerable<GetActMsgBoardDto>? ActivitiesMessageBoards { get; set; }
    }
}
