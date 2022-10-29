namespace _3TeamProject.Data
{
    public class CartApiDto
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int? UnitStock { get; set; }
        public int? UniOnOrder { get; set; }
        public int? ProductRecommendation { get; set; }
        public string? ProductIntroduce { get; set; }
        public int? ProductStatusId { get; set; }
        public int? ProductHomePage { get; set; }
        public int Amount { get; set; }
        public int SubTotal { get; set; }
        public string? ProductPicturePath { get; set; } = null!;
    }
}
