using System;
using Microsoft.Build.Framework;

namespace _3TeamProject.Areas.Administrators.Data
{
	public class UpdateProductDto
	{
        [Required]
        public int ProductCategoryID { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string? QuantityPerUnit { get; set; }
        [Required]
        public decimal ProductUnitPrice { get; set; }
        [Required]
        public int? UnitStock { get; set; }
        [Required]
        public string ProductIntroduce { get; set; } = null!;
        [Required]
        public int? ProductStatusId { get; set; }
    }
}

