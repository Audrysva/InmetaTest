using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace InmetaTest.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private static readonly List<Product> OperationalSystems= new()
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Windows 11",
                Qty = 10,
                Price = 1000,
                CreatedAt = DateTimeOffset.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Linux Cent OS",
                Qty = 20,
                Price = 10,
                CreatedAt = DateTimeOffset.UtcNow
            },
        };
        
        private static readonly List<Product> software = new()
        {
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Office 365",
                Qty = 10,
                Price = 1000,
                CreatedAt = DateTimeOffset.UtcNow
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "VS 2019",
                Qty = 20,
                Price = 10000,
                CreatedAt = DateTimeOffset.UtcNow
            },
        };

        private readonly List<Order> orders = new()
        {
            new Order
            {
                Id = Guid.NewGuid(),
                Products = OperationalSystems
            },
            new Order
            {
                Id = Guid.NewGuid(),
                Products = software
            }

        };

        private readonly string connectionString;

        private readonly IConfiguration config;

        // public OrdersRepository(IConfiguration config) {
        //     this.config = config;
        // }

        // public SqlConnection GetOpenConnection() {
        //     string cs = config["Data:DefaultConnection:ConnectionString"];
        //     SqlConnection connection = new SqlConnection(cs);
        //     connection.Open();
        //     return connection;
        // }

        public Order GetOrder(Guid id)
        {
            return orders.SingleOrDefault(order => order.Id == id);
        }

        public IEnumerable<Order> GetOrders()
        {
            return orders;
        }
    }
}
