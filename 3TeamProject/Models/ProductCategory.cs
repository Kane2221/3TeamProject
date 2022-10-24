using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
