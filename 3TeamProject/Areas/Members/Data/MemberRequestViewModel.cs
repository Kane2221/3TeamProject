using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Members.Data
{
    public class MemberRequestViewModel
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
        [Required]
        public string MemberName { get; set; }
        
        public string NickName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public string? Fax { get; set; }
        [Required]
        public string IdentityNumber { get; set; }
        [Required]
        public string CellPhoneNumber { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
