using System;

namespace InmetaTest.Entities
{
    public record Service
    {
        public int Id { get; init; }
        public Guid OrderId { get; set; }
        public EServiceTypes TypeId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}