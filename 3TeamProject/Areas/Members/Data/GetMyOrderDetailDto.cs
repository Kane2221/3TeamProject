namespace _3TeamProject.Areas.Members.Data
{
    public class GetMyOrderDetailDto
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
    }
}
