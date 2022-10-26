using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Suppliers.Data
{
    public class GetSupplierDto
    {
        public string Account { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int TaxId { get; set; }
        public string? Fax { get; set; } = string.Empty;
        public string CellPhoneNumber { get; set; } = string.Empty;
        public string SupplierPhoneNumber { get; set; } = string.Empty;
        public string SupplierPostalCode { get; set; } = string.Empty;
        public string SupplierCountry { get; set; } = string.Empty;
        public string SupplierCity { get; set; } = string.Empty;
        public string SupplierAddress { get; set; } = string.Empty;
    }
}
