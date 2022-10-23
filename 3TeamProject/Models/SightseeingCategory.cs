using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class SightseeingCategory
    {
        public SightseeingCategory()
        {
            Sightseeings = new HashSet<Sightseeing>();
        }

        public int SightseeingCategoryId { get; set; }
        public string? CategoryName { get; set; }

        public virtual ICollection<Sightseeing> Sightseeings { get; set; }
    }
}
