namespace _3TeamProject.Areas.Members.Data
{
    public class MemberGetViewModel
    {
        public string Account { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string MemberName { get; set; } = null!;
        public string? NickName { get; set; }
        public DateTime Birthday { get; set; }
        public string IdentityNumber { get; set; } = null!;
        public string CellPhoneNumber { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string PostalCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int? Age { get; set; }
    }
}
