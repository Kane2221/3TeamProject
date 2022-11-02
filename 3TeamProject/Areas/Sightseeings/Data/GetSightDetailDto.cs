namespace _3TeamProject.Areas.Sightseeings.Data
{
    public class GetSightDetailDto
    {
        public int SightseeingId { get; set; }
        public string SightseeingName { get; set; } = null!;
        public string? SightseeingCountry { get; set; }
        public string? SightseeingCity { get; set; }
        public string? SightseeingAddress { get; set; }
        public double? SightseeingScore { get; set; }
        public string? SightseeingIntroduce { get; set; }
        public string? CategoryName { get; set; }
        public virtual IEnumerable<SightMessageDto> SightseeingMessageBoards { get; set; }
        public virtual IEnumerable<GetSightPicInfoDto> SightseeingPictureInfos { get; set; }


    }
}
