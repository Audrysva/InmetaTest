using System;
using System.Collections.Generic;
using InmetaTest.Entities;


namespace InmetaTest.Dtos
{
    public record OrderDto
    {
        public Guid Id{ get; set; }
        public Customer Customer { get; set; }
        public Address AddressFrom { get; set; }
        public Address AddressTo { get; set; }
        public string OrderNotes { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
        public List<Service> Services { get; set; }

    }
}
