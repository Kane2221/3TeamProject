using Microsoft.Build.Framework;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class UpdateSightViewModel
    {
        [Required]
        public string SightseeingName { get; set; } = null!;
        [Required]
        public string? SightseeingCountry { get; set; }
        [Required]
        public string? SightseeingCity { get; set; }
        [Required]
        public string? SightseeingAddress { get; set; }
        [Required]
        public double? SightseeingScore { get; set; }
        [Required]
        public string? SightseeingIntroduce { get; set; }
        [Required]
        public int? SightseeingHomePage { get; set; }
        [Required]
        public int? SightseeingCategoryId { get; set; }
    }
}
