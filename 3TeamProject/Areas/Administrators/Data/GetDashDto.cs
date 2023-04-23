using System;
using System.Collections.Generic;

namespace _3TeamProject.Areas.Administrators.Data
{
	public class GetDashDto
	{
		public int MemberCount { get; set; }
        public int ActivityCount { get; set; }
        public int ActivityMessageCount { get; set; }
        public int UnpaymentOrderCount { get; set; }
        public int UnShipOrderCount { get; set; }
        public int UnproveProductCount { get; set; }
        public int UnproveSpplierCount { get; set; }
        public decimal MonthlySales { get; set; }
        public IEnumerable<GetSixMonthlySales>? SixMonthlySales { get; set; }
    }
    public class GetSixMonthlySales
    {
        public string? Date { get; set; }
        public decimal? Sales { get; set; }
    }
}

