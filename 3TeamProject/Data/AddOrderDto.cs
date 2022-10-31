namespace _3TeamProject.Data
{
    public class AddOrderDto
    {
        public int SupplierId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int? UnitStock { get; set; }
        public int? UniOnOrder { get; set; }
        public DateTime? AddedTime { get; set; }
        public DateTime? RemovedTime { get; set; }
        public string? ProductIntroduce { get; set; }
        public int? ProductStatusId { get; set; }
        public string ProductPictureName { get; set; } = null!;
        public IList<IFormFile> files { get; set; }

    }
}
