using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Suppliers.Data
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; }

        [Required, MinLength(8)]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ComfirmPassword { get; set; }

    }
}
