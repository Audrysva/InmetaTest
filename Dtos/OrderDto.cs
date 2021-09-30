using System;
using System.Collections.Generic;


namespace InmetaTest.Dtos
{
    public record OrderDto
    {
        public Guid Id { get; init; }
        public List<ProductDto> Products { get; set; }
    }
}
