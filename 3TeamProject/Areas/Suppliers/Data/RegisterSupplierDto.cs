﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Sppliers.Data
{
    public class RegisterSupplierDto
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password"), PasswordPropertyText]
        public string ComfirmPassword { get; set; } = string.Empty;

        [Required]
        public string ContactName { get; set; } = string.Empty;
        [Required]
        public string CompanyName { get; set; } = string.Empty;
        [Required]
        public int TaxId { get; set; }
        public string? Fax { get; set; } = string.Empty;
        [Required]
        public string CellPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string SupplierPhoneNumber { get; set; } = string.Empty;
        [Required]
        public string SupplierPostalCode { get; set; } = string.Empty;
        [Required]
        public string SupplierCountry { get; set; } = string.Empty;
        [Required]
        public string SupplierCity { get; set; } = string.Empty;
        [Required]
        public string SupplierAddress { get; set; } = string.Empty;
    }
}
