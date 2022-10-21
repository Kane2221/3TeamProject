using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class AdminGetViewModel
    {
        
        public string Account { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;

        public string Roles { get; set; } = string.Empty;

        public string AdministratorName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;
    }
}
