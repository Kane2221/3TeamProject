using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class ProductStatusCategoy
    {
        public ProductStatusCategoy()
        {
            Products = new HashSet<Product>();
        }

        public int ProductStatusId { get; set; }
        public string StatusName { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
