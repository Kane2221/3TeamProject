using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class SightseeingPictureInfo
    {
        public int SightseeingPictureId { get; set; }
        public int SightseeingId { get; set; }
        public string SightseeingPicturePath { get; set; } = null!;
        public string SightseeingPictureName { get; set; } = null!;

        public virtual Sightseeing Sightseeing { get; set; } = null!;
    }
}
