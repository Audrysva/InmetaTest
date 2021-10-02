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

        private static List<Order> _orderList = new();

        public OrdersRepository(IConfiguration config)
        {
            this.config = config;
        }

        public SqlConnection GetOpenConnection()
        {
            string cs = config.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }

        public Order GetOrder(Guid id)
        {
            _orderList = new();
            var order = new Order();
            var conn = GetOpenConnection();
            var reader = SelectOrder(conn, id);

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
        }

        public IEnumerable<Order> GetOrders()
        {
            _orderList = new();

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
            SaveOrder(conn, order);
            conn.Close();
        }

        public void DeleteOrder(Guid id)
        {
            throw new NotImplementedException();
        }

        private void SaveOrder(SqlConnection conn, Order order)
        {
            string query =
                @"INSERT INTO orders (Id,CreatedAt) VALUES (NEWID(),CURRENT_TIMESTAMP)";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
            cmd.Parameters["@Id"].Value = order.Id;

            cmd.ExecuteReader();

            foreach (var product in order.Services)
            {
                InsertProductToDb(conn, order.Id, product);
            }
        }

        private static void InsertProductToDb(SqlConnection conn, Guid orderId, Service service)
        {
            string query =
                "INSERT INTO Products (Id,CreatedAt,OrderId,Name,Qty,Price) VALUES (NEWID(),CURRENT_TIMESTAMP,@OrderId,@Name,@Qty,@Price);";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add("@OrderId", System.Data.SqlDbType.UniqueIdentifier);
            cmd.Parameters["@OrderId"].Value = orderId;


            SqlDataReader dr = cmd.ExecuteReader();
        }


        private static SqlDataReader SelectOrders(SqlConnection conn)
        {
            string query =
                @"SELECT Id,CustomerId,CustomerPhoneNumber,CustomerEmail,CustomerName,AddressFromId,AddressToId,OrderNotes,ServiceTypeId,DateFrom,DateTo,FromZip,FromCity,FromStreet,FromNumber,FromCountryCode,ToZip,ToCity,ToStreet,ToNumber,ToCountryCode FROM v_Orders";

            SqlCommand cmd = new SqlCommand(query, conn);

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }

        private static List<Order> OrdersOf(SqlDataReader reader)
        {
            var orderId = (Guid)reader["Id"];

            var existingOrder = _orderList.Where(x => x.Id == orderId);
            if (existingOrder.Any())
            {
                // existingOrder.FirstOrDefault().Services.Add(product);
            }
            else
            {
                var order = ReaderToOrder(reader);
                //order.Services.Add(service);
                _orderList.Add(order);
            }

            return _orderList;
        }

        private static Order ReaderToOrder(SqlDataReader dr)
        {
            var order = new Order
            {
                Id = (Guid)dr["Id"],
                CustomerId = (Guid)dr["CustomerId"],
                PhoneNumber = (string)dr["CustomerPhoneNumber"],
                CustomerName = (string)dr["CustomerName"],
                EmailAddress = (string)dr["CustomerEmail"],
                AddressFromId = (Guid)dr["AddressFromId"],
                AddressToId = (Guid)dr["AddressToId"],
                ServiceTypeId = (Int16)dr["ServiceTypeId"],
                DateFrom = (DateTime)dr["DateFrom"],
                DateTo = (DateTime)dr["DateTo"],
                FromZip = (string)dr["FromZip"],
                FromCity = (string)dr["FromCity"],
                FromStreet = (string)dr["FromStreet"],
                FromNumber = (string)dr["FromNumber"],
                FromCountryCode = (string)dr["FromCountryCode"],
                ToZip = (string)dr["ToZip"],
                ToCity = (string)dr["ToCity"],
                ToStreet = (string)dr["ToStreet"],
                ToNumber = (string)dr["ToNumber"],
                ToCountryCode = (string)dr["ToCountryCode"],
                OrderNotes = (string)dr["OrderNotes"],
                Services = new()
            };
            return order;
        }

        private static SqlDataReader SelectOrder(SqlConnection conn, Guid id)
        {
            string query =
                "SELECT o.Id AS OrderId, p.Id AS ProductId,p.Name,p.Qty,p.Price  FROM orders o JOIN products p on p.OrderID=o.id WHERE  o.Id=@Id";

            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
            cmd.Parameters["@Id"].Value = id;

            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }


        private static Service ReaderToProduct(SqlDataReader dr)
        {
            var product = new Service
            {
                Id = dr.GetInt32(1)
            };
            return product;
        }
    }
}