using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Sppliers.Data
{
    public class SupplierRegisterRequest
    {
        [Required]
        public string Account { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ComfirmPassword { get; set; }

        [Required]
        public int Roles { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public int TaxId { get; set; }
        public string? Fax { get; set; }
        public string CellPhoneNumber { get; set; }
        public string SupplierPhnoeNumber { get; set; }
        public string SupplierPostalCode { get; set; }
        public string SupplierCountry { get; set; }
        public string SupplierCity { get; set; }
        public string SupplierAddress { get; set; }
    }
}
