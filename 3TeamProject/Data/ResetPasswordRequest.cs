using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Data
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required, MinLength(8, ErrorMessage = "密碼少於8位數")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password", ErrorMessage = "密碼不一致")]
        public string ComfirmPassword { get; set; } = string.Empty;

    }
}