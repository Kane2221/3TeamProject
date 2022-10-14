using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class OrderStatusCategory
    {
        public OrderStatusCategory()
        {
            Orders = new HashSet<Order>();
        }

        public int OrderCategoryId { get; set; }
        public string OrderCategoryName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
