using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Members.Data
{
    public class MemberRegisterViewModel
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
        public int Roles { get; set; }
        [Required]
        public string MemberName { get; set; } = string.Empty;

        public string NickName { get; set; } = string.Empty;
        [Required]
        public DateTime Birthday { get; set; }
        public string? Fax { get; set; }
        [Required]
        public string IdentityNumber { get; set; } = string.Empty;
        [Required]
        public string CellPhoneNumber { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string Country { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        [Required]
        public string Address { get; set; } = string.Empty;
    }
}
