namespace _3TeamProject.Areas.Suppliers.Data
{
    public class GetProductDto
    {
        public int ProductId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int? UnitStock { get; set; }
        public int? UniOnOrder { get; set; }
        public int? ProductRecommendation { get; set; }
        public string StatusName { get; set; } = null!;
    }
}

