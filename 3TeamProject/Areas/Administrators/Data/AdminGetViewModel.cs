using System.ComponentModel.DataAnnotations;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class AdminGetViewModel
    {
        
        public string Account { get; set; }

        
        public string Email { get; set; }

        public string Roles { get; set; }
        
        public string AdministratorName { get; set; }
        
        public string PhoneNumber { get; set; } 
    }
}
