using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmetaTest.Entities
{
    public record Order
    {
        public Guid Id{ get; init; }
        public List<Product> Products { get; set; }
    }
}
