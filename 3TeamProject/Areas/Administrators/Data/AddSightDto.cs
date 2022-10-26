using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class AddSightDto
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
        public int? SightseeingCategoryId { get; set; }
        public IList<IFormFile>? Files { get; set; }


    }
}
