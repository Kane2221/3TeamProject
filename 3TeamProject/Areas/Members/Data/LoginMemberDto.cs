using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Members.Data
{
    public class LoginMemberDto
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
