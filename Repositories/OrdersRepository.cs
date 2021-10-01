using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.Extensions.Configuration;

namespace InmetaTest.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly string connectionString;

        private readonly IConfiguration config;
        
        private static List<Order> _orderList = new ();

        public OrdersRepository(IConfiguration config) {
            this.config = config;
        }
        public SqlConnection GetOpenConnection() {
            string cs = config.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        public Order GetOrder(Guid id)
        {
            _orderList = new ();
            var order = new Order();
            var conn = GetOpenConnection();
            var reader = SelectOrder(conn,id);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    order = OrdersOf(reader).FirstOrDefault();
                }
            }
            
            reader.Close();
            
            conn.Close();
            
            return order;
            //return orders.SingleOrDefault(order => order.Id == id);
        }

        public IEnumerable<Order> GetOrders()
        {
            _orderList = new ();
            
            var conn = GetOpenConnection();
            
            var reader = SelectOrders(conn);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _orderList = OrdersOf(reader);
                }
            }
            
            reader.Close();
            
            conn.Close();
            
            return _orderList.ToArray();
        }

        public void CreateOrder(Order order)
        {
            var conn = GetOpenConnection();
            SaveOrder(conn,order);
            conn.Close();
        }

        private void SaveOrder(SqlConnection conn, Order order)
        {
            string query =
                @"INSERT INTO orders (Id,CreatedAt) VALUES (NEWID(),CURRENT_TIMESTAMP)";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.ExecuteReader();

            query = "INSERT INTO Products (Id,CreatedAt,OrderId,Name,Qty,Price) VALUES (NEWID(),CURRENT_TIMESTAMP,scope_identity(),@Name,@Qty,@Price);";
            
            cmd = new SqlCommand(query, conn);
            
            cmd.Parameters.Add("@Name",System.Data.SqlDbType.VarChar);
            cmd.Parameters["@Name"].Value = order.Products.FirstOrDefault().Name;    
            
            cmd.Parameters.Add("@Qty",System.Data.SqlDbType.Int);
            cmd.Parameters["@Qty"].Value = order.Products.FirstOrDefault().Qty;  
            
            cmd.Parameters.Add("@Price",System.Data.SqlDbType.Decimal);
            cmd.Parameters["@Price"].Value = order.Products.FirstOrDefault().Price;

            SqlDataReader dr = cmd.ExecuteReader();
        }


        private static List<Order> OrdersOf(SqlDataReader reader)
        {
            var orderId = reader.GetGuid(0);
            var product = ReaderToProduct(reader);
            var existingOrder = _orderList.Where(x => x.Id == orderId);
            if (existingOrder.Any())
            {
                existingOrder.FirstOrDefault().Products.Add(product);
            }
            else
            {
                var order = ReaderToOrder(reader);
                order.Products.Add(product);
                _orderList.Add(order);
            }

            return _orderList;
        }

        private static SqlDataReader SelectOrders(SqlConnection conn)
        {
            string query =
                @"SELECT o.Id AS OrderId, p.Id AS ProductId,p.Name,p.Qty,p.Price  FROM orders o JOIN products p on p.OrderID=o.id";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        } 
        
        private static SqlDataReader SelectOrder(SqlConnection conn, Guid id)
        {
            string query =
                "SELECT o.Id AS OrderId, p.Id AS ProductId,p.Name,p.Qty,p.Price  FROM orders o JOIN products p on p.OrderID=o.id WHERE  o.Id=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);
            
            cmd.Parameters.Add("@Id",System.Data.SqlDbType.UniqueIdentifier);
            cmd.Parameters["@Id"].Value = id;    

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        private static Order ReaderToOrder(SqlDataReader dr)
        {
            var order = new Order
            {
                Id = dr.GetGuid(0),
                Products = new()
            };
            return order;
        }

        private static Product ReaderToProduct(SqlDataReader dr)
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
