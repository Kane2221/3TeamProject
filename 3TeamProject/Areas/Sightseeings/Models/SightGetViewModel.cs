namespace _3TeamProject.Areas.Sightseeings.Models
{
    public class SightGetViewModel
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
    }
}
