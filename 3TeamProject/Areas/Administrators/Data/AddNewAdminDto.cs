using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class AddNewAdminDto
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password"), PasswordPropertyText]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public int Roles { get; set; }
        [Required]
        public string AdministratorName { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
