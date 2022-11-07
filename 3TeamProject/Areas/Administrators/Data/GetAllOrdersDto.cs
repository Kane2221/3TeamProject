using _3TeamProject.Models;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetAllOrdersDto
    {
        public int OrderId { get; set; }
        public int MemberId { get; set; }
        public int? AdministratorId { get; set; }
        public string? OrderDate { get; set; }
        public string? ShipDate { get; set; }
        public string OrderCategoryName { get; set; } = null!;
        public string ShipPostalCode { get; set; } = null!;
        public string ShipCountry { get; set; } = null!;
        public string ShipCity { get; set; } = null!;
        public string ShipAddress { get; set; } = null!;
        public string PaymentCategoryName { get; set; } = null!;
        public string ShipCategoryName { get; set; } = null!;
        public decimal Total { get; set; }
        public virtual IEnumerable<GetOrderDetailDto> OrderDetails { get; set; }
    }
}
