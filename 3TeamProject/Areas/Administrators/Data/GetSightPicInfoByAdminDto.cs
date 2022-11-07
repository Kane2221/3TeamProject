namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetSightPicInfoByAdminDto
    {
        public int SightseeingPictureId { get; set; }
        public string SightseeingPicturePath { get; set; } = null!;
        public string SightseeingPictureName { get; set; } = null!;
    }
}
