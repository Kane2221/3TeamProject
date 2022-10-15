using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Data
{
    public class LoginReuqest
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
