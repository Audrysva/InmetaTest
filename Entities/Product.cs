using System;

namespace InmetaTest.Entities
{
    public record Product
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public double Qty { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}