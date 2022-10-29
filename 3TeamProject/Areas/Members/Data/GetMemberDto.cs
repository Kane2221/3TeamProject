namespace _3TeamProject.Areas.Members.Data
{
    public class GetMemberDto
    {
        public string Account { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string? NickName { get; set; }
        public DateTime Birthday { get; set; }
        public string IdentityNumber { get; set; } = string.Empty;
        public string CellPhoneNumber { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int? Age { get; set; }
    }
}
