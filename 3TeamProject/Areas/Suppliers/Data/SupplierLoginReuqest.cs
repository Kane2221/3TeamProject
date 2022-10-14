using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Sppliers.Data
{
    public class SupplierLoginReuqest
    {
        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
