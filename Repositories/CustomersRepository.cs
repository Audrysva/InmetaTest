using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using InmetaTest.Entities;
using Microsoft.Extensions.Configuration;

namespace InmetaTest.Repositories
{
    public class CustomersRepository :ICustomersRepository
    {
        private readonly string connectionString;

        private readonly IConfiguration config;
        
        public CustomersRepository(IConfiguration config) {
            this.config = config;
        }
        
        public SqlConnection GetOpenConnection() {
            string cs = config.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
        
        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = new ();
            
            var conn = GetOpenConnection();
            
            var reader = SelectCustomers(conn);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var customer = new Customer()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        PhoneNumber = reader.GetString(2),
                        EmailAddress = reader.GetString(3),
                        CreatedAt = reader.GetDateTime(4)
                    };
                    customers.Add(customer);
                }
            }
            
            reader.Close();
            
            conn.Close();
            
            return customers.ToArray();
        }

        private SqlDataReader SelectCustomers(SqlConnection conn)
        {
            try
            {
                string query =
                    @"SELECT Id, Name, PhoneNumber, EmailAddress, CreatedAt  FROM Customers";

                var cmd = new SqlCommand(query, conn);

                var dr = cmd.ExecuteReader();
                return dr;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new InvalidDataException();
            }
        }
    }
}