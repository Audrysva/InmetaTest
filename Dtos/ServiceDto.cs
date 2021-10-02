using System;


namespace InmetaTest.Dtos
{
    public record ServiceDto
    {
        public int Id { get; init; }

        public DateTimeOffset CreatedAt { get; init; }
    }
}
