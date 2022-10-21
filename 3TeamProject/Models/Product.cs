using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductsMessageBoards = new HashSet<ProductsMessageBoard>();
            ProductsPictureInfos = new HashSet<ProductsPictureInfo>();
        }

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
<<<<<<< HEAD
        public string? ProductIntroduce { get; set; }
=======
        public string ProductIntroduce { get; set; } = null!;
>>>>>>> e1af6f3e8a20b9717349e0b46dfb6f09d329bbe6

        public virtual ProductCategory ProductCategory { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductsMessageBoard> ProductsMessageBoards { get; set; }
        public virtual ICollection<ProductsPictureInfo> ProductsPictureInfos { get; set; }
    }
}
