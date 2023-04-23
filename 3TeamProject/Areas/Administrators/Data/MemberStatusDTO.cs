using Microsoft.Build.Framework;

namespace _3TeamProject.Areas.Administrators.Data
{
    public class MemberStatusDTO
    {
        [Required]
        public string? MemberStatus { get; set; }
    }
}
