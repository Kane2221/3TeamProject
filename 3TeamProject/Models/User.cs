using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class User
    {
        public User()
        {
            ActivitiesMessageBoards = new HashSet<ActivitiesMessageBoard>();
            Administrators = new HashSet<Administrator>();
            Members = new HashSet<Member>();
            SightseeingMessageBoards = new HashSet<SightseeingMessageBoard>();
            Suppliers = new HashSet<Supplier>();
        }

        public int UserId { get; set; }
        public string Account { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public int? Roles { get; set; }
        public string VerificationToken { get; set; } = null!;
        public string? PasswordResetToken { get; set; }
        public DateTime? VerfiedAt { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public string? PicturePath { get; set; }

        public virtual Role? RolesNavigation { get; set; }
        public virtual ICollection<ActivitiesMessageBoard> ActivitiesMessageBoards { get; set; }
        public virtual ICollection<Administrator> Administrators { get; set; }
        public virtual ICollection<Member> Members { get; set; }
        public virtual ICollection<SightseeingMessageBoard> SightseeingMessageBoards { get; set; }
        public virtual ICollection<Supplier> Suppliers { get; set; }
    }
}
