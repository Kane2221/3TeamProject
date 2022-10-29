using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Member
    {
        public Member()
        {
            JoinMembers = new HashSet<JoinMember>();
            Orders = new HashSet<Order>();
            ProductsMessageBoards = new HashSet<ProductsMessageBoard>();
            SocialActivities = new HashSet<SocialActivity>();
        }

        public int MemberId { get; set; }
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
        public int UserId { get; set; }
        public int? Age { get; set; }
        public int? MemberStatusId { get; set; }

        public virtual MemberStatusCategory? MemberStatus { get; set; }
        public virtual User User { get; set; } = null!;
        public virtual ICollection<JoinMember> JoinMembers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ProductsMessageBoard> ProductsMessageBoards { get; set; }
        public virtual ICollection<SocialActivity> SocialActivities { get; set; }
    }
}
