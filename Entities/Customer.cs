using System;

namespace InmetaTest.Entities
{
    public record Customer
    {
        public Guid Id{ get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}