using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Order
    {

        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }


        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int? AdministratorId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentStatus { get; set; }
        public int ShipStatus { get; set; }
        public string ShipPostalCode { get; set; } = null!;
        public string ShipCountry { get; set; } = null!;
        public string ShipCity { get; set; } = null!;
        public string ShipAddress { get; set; } = null!;


        public virtual Administrator? Administrator { get; set; }
        public virtual Member Member { get; set; } = null!;
        public virtual OrderStatusCategory OrderStatusNavigation { get; set; } = null!;
        public virtual PaymentStatusCategory PaymentStatusNavigation { get; set; } = null!;
        public virtual ShipStatusCategory ShipStatusNavigation { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
