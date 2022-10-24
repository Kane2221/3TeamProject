using System;
using System.Collections.Generic;

namespace _3TeamProject.Models
{
    public partial class AdministratorStatusCategory
    {
        public AdministratorStatusCategory()
        {
            Administrators = new HashSet<Administrator>();
        }

        public int AdministratorStatusId { get; set; }
        public string StatusName { get; set; } = null!;

        public virtual ICollection<Administrator> Administrators { get; set; }
    }
}
