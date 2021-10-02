using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmetaTest.Entities
{
    public record Order
    {
        public Guid Id{ get; init; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Guid AddressFromId { get; set; }
        public Guid AddressToId { get; set; }
        public int ServiceTypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string FromZip { get; set; }
        public string FromCity { get; set; }
        public string FromStreet { get; set; }
        public string FromNumber { get; set; }
        public string FromCountryCode { get; set; }
        public string ToZip { get; set; }
        public string ToCity { get; set; }
        public string ToStreet { get; set; }
        public string ToNumber { get; set; }
        public string ToCountryCode { get; set; }
        public string OrderNotes { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
        public List<Service> Services { get; set; }
    }
}
