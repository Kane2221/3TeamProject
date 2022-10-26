using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class SupplierStatusCategoy
    {
        public SupplierStatusCategoy()
        {
            Suppliers = new HashSet<Supplier>();
        }

        public int SupplierStatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
