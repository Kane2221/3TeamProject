using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Supplier
    {
        public Supplier()
        {
            Products = new HashSet<Product>();
        }

        public int SuppliersId { get; set; }
        public string ContactName { get; set; } = null!;
        public string? CompanyName { get; set; }
        public int TaxId { get; set; }
        public string? Fax { get; set; }
        public string CellPhoneNumber { get; set; } = null!;
        public string SupplierPhoneNumber { get; set; } = null!;
        public string SupplierPostalCode { get; set; } = null!;
        public string SupplierCountry { get; set; } = null!;
        public string SupplierCity { get; set; } = null!;
        public string SupplierAddress { get; set; } = null!;
        public int Id { get; set; }

        public virtual User IdNavigation { get; set; } = null!;
        public virtual ICollection<Product> Products { get; set; }
    }
}
