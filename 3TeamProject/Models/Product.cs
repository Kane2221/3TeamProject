using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int ProductCategoryId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public int UnitStock { get; set; }
        public int? UniOnOrder { get; set; }
        public int? ProductRecommendation { get; set; }
        public DateTime? AddedTime { get; set; }
        public DateTime? RemovedTime { get; set; }
        public string ProductStatus { get; set; } = null!;
    }
}
