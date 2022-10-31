using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace _3TeamProject.Data
{
    public class VerifyDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required, MinLength(8), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password"), PasswordPropertyText]
        public string ComfirmPassword { get; set; } = string.Empty;
    }
}
