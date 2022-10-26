using _3TeamProject.Areas.Administrators.Data;

namespace _3TeamProject.Areas.Members.Data
{
    public class GetMyOrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public string OrderCategoryName { get; set; } = null!;
        public string PaymentCategoryName { get; set; } = null!;
        public string ShipCategoryName { get; set; } = null!;
        public virtual IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
