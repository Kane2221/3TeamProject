namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetAllProductDto
    {
        public int ProductId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int? UnitStock { get; set; }
        public int? UniOnOrder { get; set; }
        public int? ProductRecommendation { get; set; }
        public string? AddedTime { get; set; }
        public string? RemovedTime { get; set; }
        public string? ProductIntroduce { get; set; }
        public string StatusName { get; set; } = null!;
        public int? ProductHomePage { get; set; }
    }
}
