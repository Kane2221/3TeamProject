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

    }
}
