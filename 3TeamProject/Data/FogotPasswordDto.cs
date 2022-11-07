using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Data
{
    public class FogotPasswordDto
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
