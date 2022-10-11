using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class PaymentStatusCategory
    {
        public PaymentStatusCategory()
        {
            Orders = new HashSet<Order>();
        }

        public int PaymentCategoryId { get; set; }
        public string PaymentCategoryName { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
