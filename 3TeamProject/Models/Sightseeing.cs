using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Sightseeing
    {
        public Sightseeing()
        {
            SightseeingMessageBoards = new HashSet<SightseeingMessageBoard>();
            SightseeingPictureInfos = new HashSet<SightseeingPictureInfo>();
        }

        public int SightseeingId { get; set; }
        public string SightseeingName { get; set; } = null!;
        public string? SightseeingCountry { get; set; }
        public string? SightseeingCity { get; set; }
        public string? SightseeingAddress { get; set; }
        public double? SightseeingScore { get; set; }
        public int AdministratorId { get; set; }

        public virtual Administrator Administrator { get; set; } = null!;
        public virtual ICollection<SightseeingMessageBoard> SightseeingMessageBoards { get; set; }
        public virtual ICollection<SightseeingPictureInfo> SightseeingPictureInfos { get; set; }
    }
}
