using _3TeamProject.Models;

namespace _3TeamProject.Areas.Sightseeings.Data
{
    public class SightPicInfoViewModel
    {
        public int SightseeingPictureId { get; set; }
        public string SightseeingPicturePath { get; set; } = null!;
        public string SightseeingPictureName { get; set; } = null!;

    }
}
