using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class GetAdminRequest
    {
        public string Account { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ComfirmPassword { get; set; } = string.Empty;

        public int RoleName { get; set; }
        public string AdministratorName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
