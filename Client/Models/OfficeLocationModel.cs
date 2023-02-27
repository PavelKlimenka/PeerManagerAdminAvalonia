using System;

namespace Client.Models
{
    public class OfficeLocationModel
    {
        public Guid Id { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string OfficeCode { get; set; }
        public string? OfficeImage { get; set; }
    }
}
