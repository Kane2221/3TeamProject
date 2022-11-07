using Microsoft.Build.Framework;

namespace _3TeamProject.Areas.Suppliers.Data
{
    public class AddProductDto
    {
        [Required]
        public string CategoryName { get; set; } = null!;
        [Required]
        public string ProductName { get; set; } = null!;
        public string? QuantityPerUnit { get; set; }
        [Required]
        public decimal ProductUnitPrice { get; set; }
        [Required]
        public int ProductCategoryId { get; set; }
        public int? UnitStock { get; set; }
        [Required]
        public string? ProductIntroduce { get; set; }
        public IList<IFormFile>? ProductFiles { get; set; }
    }
}
