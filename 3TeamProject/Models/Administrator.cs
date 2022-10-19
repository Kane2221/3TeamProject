using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class Administrator
    {
        public Administrator()
        {
            Orders = new HashSet<Order>();
            Sightseeings = new HashSet<Sightseeing>();
        }

        public int AdministratorId { get; set; }
        public string AdministratorName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Sightseeing> Sightseeings { get; set; }
    }
}
