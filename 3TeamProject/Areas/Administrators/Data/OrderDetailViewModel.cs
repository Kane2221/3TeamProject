﻿namespace _3TeamProject.Areas.Administrators.Data
{
    public class OrderDetailViewModel
    {
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
    }
}
