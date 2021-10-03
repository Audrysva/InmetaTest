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
        public static ServiceDto AsDto(this Service service)
        {
            return new ServiceDto
            {
                Id = service.Id,
                CreatedAt = service.CreatedAt
            };
        }

        public static OrderDto AsDto(this Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                Customer = new Customer()
                {
                    Id = order.CustomerId,
                    Name = order.CustomerName,
                    PhoneNumber = order.PhoneNumber,
                    EmailAddress = order.EmailAddress
                },
                AddressFrom = new Address()
                {
                    Id = order.AddressFromId,
                    Zip = order.FromZip,
                    City = order.FromCity,
                    Street = order.FromStreet,
                    Number = order.FromNumber,
                    CountryCode = order.FromCountryCode
                },
                AddressTo = new Address()
                {
                    Id = order.AddressToId,
                    Zip = order.ToZip,
                    City = order.ToCity,
                    Street = order.ToStreet,
                    Number = order.ToNumber,
                    CountryCode = order.ToCountryCode
                },
                Services = order.Services,
                OrderNotes = order.OrderNotes
            };
        }
    }
}