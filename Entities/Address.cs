using System;

namespace InmetaTest.Entities
{
    public record Address
    {
        public Guid Id{ get; init; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string CountryCode { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}