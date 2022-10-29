using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class UpdateMemberPasswordDto
    {
        [Required, MinLength(8), PasswordPropertyText]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password"), PasswordPropertyText]
        public string ComfirmPassword { get; set; } = string.Empty;

    }
}
