using InmetaTest.Dtos;
using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace InmetaTest.Helpers
{
    public static class Extentions
    {
        public static ProductDto AsDto(this Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Qty = product.Qty,
                Price = product.Price,
                CreatedAt = product.CreatedAt
            };
        }

        public static OrderDto AsDto(this Order order)
        {
            var products = order.Products.Select(product => product.AsDto()).ToList();
            return new OrderDto
            {
                Id = order.Id,
                Products = products
            };
        }

    }
}
