using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class AdminLoginReuqest
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
