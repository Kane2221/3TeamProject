using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class ShipStatusCategory
    {
        public ShipStatusCategory()
        {
            Orders = new HashSet<Order>();
        }

        public int ShipCategoryId { get; set; }
        public string ShipCategoryName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
