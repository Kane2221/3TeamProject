using _3TeamProject.Areas.Sightseeings.Data;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetSightByAdminDto
    {
        public int SightseeingId { get; set; }
        public string SightseeingName { get; set; } = null!;
        public string? SightseeingCountry { get; set; }
        public string? SightseeingCity { get; set; }
        public string? SightseeingAddress { get; set; }
        public double? SightseeingScore { get; set; }
        public string? CategoryName { get; set; }
        public int? SightseeingHomePage { get; set; }
        public virtual IEnumerable<GetSightPicInfoByAdmin> SightseeingPictureInfos { get; set; }
    }
}