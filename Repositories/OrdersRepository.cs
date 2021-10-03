using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using InmetaTest.Dtos;
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

            return _orderList;
        }


        public void CreateOrder(OrderDto orderDto)
        {
            try
            {
                var conn = GetOpenConnection();
                var customerId = EnsureCustomer(conn, orderDto.Customer);
                var addressFromId = EnsureAddress(conn, orderDto.AddressFrom);
                var addressToId = EnsureAddress(conn, orderDto.AddressTo);
                Order order = new Order()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    AddressFromId = addressFromId,
                    AddressToId = addressToId,
                    OrderNotes = orderDto.OrderNotes
                };
                AddOrder(conn, order);
                AddServices(conn, order.Id, order.Services);
                conn.Close();
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
        }

        public void UpdateOrder(OrderDto order)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrder(Guid id)
        {
            try
            {
                var conn = GetOpenConnection();
                string query = "DELETE FROM Orders WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@Id"].Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
                conn.Close();
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
        }

        private void AddServices(SqlConnection conn, Guid orderId, List<Service> orderServices)
        {
            throw new NotImplementedException();
        }


        private Guid AddOrder(SqlConnection conn, Order order)
        {
            throw new NotImplementedException();
        }

        private Guid EnsureAddress(SqlConnection conn, Address address)
        {
            //Not implemented
            var addressId = address.Id == Guid.Empty ? new Guid() : address.Id;

            return addressId;
        }

        private Guid EnsureCustomer(SqlConnection conn, Customer customer)
        {
            try
            {
                var customerId = (customer.Id == Guid.Empty) ? new Guid() : customer.Id;
                string query = (customer.Id == Guid.Empty)
                    ? "INSERT INTO Customers (Id,Name,PhoneNumber,EmailAddress) VALUES (@Id,@Name,@PhoneNumber,@EmailAddress)"
                    : "UPDATE Customers SET Name = @name WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@Id"].Value = customerId;

                cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@Name"].Value = customer.Name;

                cmd.Parameters.Add("@PhoneNumber", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@PhoneNumber"].Value = customer.PhoneNumber;

                cmd.Parameters.Add("@EmailAddress", System.Data.SqlDbType.VarChar);
                cmd.Parameters["@EmailAddress"].Value = customer.EmailAddress;


                SqlDataReader dr = cmd.ExecuteReader();
                return customerId;
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
        }

        private static SqlDataReader SelectOrders(SqlConnection conn)
        {
            try
            {
                string query =
                    @"SELECT Id,CustomerId,CustomerPhoneNumber,CustomerEmail,CustomerName,AddressFromId,AddressToId,
                        OrderNotes,ServiceTypeId,DateFrom,DateTo,FromZip,FromCity,FromStreet,FromNumber,
                        FromCountryCode,ToZip,ToCity,ToStreet,ToNumber,ToCountryCode 
                    FROM v_Orders";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
        }

        private static SqlDataReader SelectOrder(SqlConnection conn, Guid id)
        {
            try
            {
                string query =
                    "SELECT o.Id AS OrderId, p.Id AS ProductId,p.Name,p.Qty,p.Price " +
                    " FROM orders o JOIN products p on p.OrderID=o.id WHERE  o.Id=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.Add("@Id", System.Data.SqlDbType.UniqueIdentifier);
                cmd.Parameters["@Id"].Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
                return dr;
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
        }

        private static List<Order> OrdersOf(SqlDataReader reader)
        {
            try
            {
                var orderId = (Guid)reader["Id"];
                Service service = new Service()
                {
                    OrderId = orderId,
                    TypeId = (EServiceTypes)(Int16)reader["ServiceTypeId"],
                    DateFrom = (DateTime)reader["DateFrom"],
                    DateTo = (DateTime)reader["DateTo"]
                };
                var existingOrder = _orderList.Find(x => x.Id == orderId);
                if (existingOrder is null)
                {
                    var order = ReaderToOrder(reader);
                    order.Services.Add(service);
                    _orderList.Add(order);
                }
                else
                {
                    existingOrder.Services.Add(service);
                }

                return _orderList;
            }
            catch (Exception e)
            {
                throw new InvalidDataException();
            }
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
    }
}