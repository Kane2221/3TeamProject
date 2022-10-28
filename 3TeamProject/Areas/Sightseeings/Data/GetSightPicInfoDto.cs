namespace _3TeamProject.Areas.Sightseeings.Data
{
    public class GetSightPicInfoDto
    {
        public int SightseeingPictureId { get; set; }
        public string SightseeingPicturePath { get; set; } = null!;
        public string SightseeingPictureName { get; set; } = null!;

    }
}
