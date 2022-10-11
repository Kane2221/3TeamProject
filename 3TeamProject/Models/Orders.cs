using System;
using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Models
{
    public class Orders
    {
        public int? OrderID { get; set; }
        public int MemberID { get; set; }
        public int Administrator_ID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int OrderStatus { get; set; }
        public int PaymentStatus { get; set; }
        public int ShipStatus { get; set; }
        public string ShipPostal_code { get; set; }
        public string ShipCountry { get; set; }
        public string ShipCity { get; set; }
        public string ShipAddress { get; set; }
    }
}
