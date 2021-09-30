using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using InmetaTest.Dtos;
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

        public OrdersRepository(IConfiguration config) {
            this.config = config;
        }
        public SqlConnection GetOpenConnection() {
            // string cs = config["Data:DefaultConnection:ConnectionString"];
            string cs = config.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        public Order GetOrder(Guid id)
        {
            var sqlConnection = GetOpenConnection();
            return orders.SingleOrDefault(order => order.Id == id);
        }

        public IEnumerable<Order> GetOrders()
        {
            var conn = GetOpenConnection();
            
            string query = @"SELECT o.Id AS OrderId, p.Id AS ProductId,p.Name,p.Qty,p.Price  FROM orders o JOIN products p on p.OrderID=o.id";
            
            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            var orderList = new List<Order>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    var orderId = dr.GetGuid(0);
                    var product = CreateProduct(dr);
                    var existingOrder = orderList.Where(x => x.Id == orderId);
                    if (existingOrder.Any())
                    {
                        existingOrder.FirstOrDefault().Products.Add(product);
                    }
                    else
                    {
                        var order = CreateOrder(dr);
                        order.Products.Add(product);
                        orderList.Add(order);
                    }
                }
            }
            
            dr.Close();
            
            conn.Close();
            return orderList.ToArray();
        }

        private static Order CreateOrder(SqlDataReader dr)
        {
            var order = new Order
            {
                Id = dr.GetGuid(0),
                Products = new()
            };
            return order;
        }

        private static Product CreateProduct(SqlDataReader dr)
        {
            var product = new Product
            {
                Id = dr.GetGuid(1),
                Name = dr.GetString(2),
                Qty = dr.GetInt32(3),
                Price = dr.GetDecimal(4)
            };
            return product;
        }
    }
}
