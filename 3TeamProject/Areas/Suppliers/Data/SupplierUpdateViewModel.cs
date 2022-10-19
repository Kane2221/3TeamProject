using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Suppliers.Data
{
    public class SupplierUpdateViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ComfirmPassword { get; set; }

        [Required]
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public int TaxId { get; set; }
        public string? Fax { get; set; }
        [Required]
        public string CellPhoneNumber { get; set; }
        [Required]
        public string SupplierPhoneNumber { get; set; } 
        [Required]
        public string SupplierPostalCode { get; set; } 
        [Required]
        public string SupplierCountry { get; set; }
        [Required]
        public string SupplierCity { get; set; } 
        [Required]
        public string SupplierAddress { get; set; }
    }
}
