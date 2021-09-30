using System;


namespace InmetaTest.Dtos
{
    public record ProductDto
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public double Qty { get; set; }
        public decimal Price { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}
