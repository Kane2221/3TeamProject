﻿namespace _3TeamProject.Data
{
    public class PayDto
    {
        public string? MemberName { get; set; }
        public string? CellPhoneNumber { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string ordernumber { get; set; }
        public string ShipPostalCode { get; set; }
        public string? ShipCountry { get; set; }
        public string ShipCity { get; set; }
        public string ShipAddress { get; set; }
        public int SubTotal { get; set; }
    }
}
