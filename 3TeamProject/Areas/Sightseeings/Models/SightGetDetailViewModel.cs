namespace _3TeamProject.Areas.Sightseeings.Models
{
    public class SightGetDetailViewModel
    {
        public int SightseeingId { get; set; }
        public string SightseeingName { get; set; } = null!;
        public string? SightseeingCountry { get; set; }
        public string? SightseeingCity { get; set; }
        public string? SightseeingAddress { get; set; }
        public double? SightseeingScore { get; set; }
        public int SightseeingPictureId { get; set; }
        public string SightseeingPicturePath { get; set; } = null!;
        public string SightseeingPictureName { get; set; } = null!;
        public string? SightseeingIntroduce { get; set; }
        public int MessageBoardId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public string? SightseeingMessageContent { get; set; }
        public DateTime MessageCreatedTime { get; set; }
    }
}
