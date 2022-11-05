namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetOrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }
    }
}
