using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using InmetaTest.Entities;
using Microsoft.Extensions.Configuration;

namespace InmetaTest.Repositories
{
    public class AddressesRepository : IAddressesRepository
    {
        private readonly string connectionString;

        private readonly IConfiguration config;
        
        public AddressesRepository(IConfiguration config) {
            this.config = config;
        }
        
        public SqlConnection GetOpenConnection() {
            string cs = config.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
        
        public IEnumerable<Address> GetAddresses()
        {
            List<Address> addresses = new ();
            
            var conn = GetOpenConnection();
            
            var reader = SelectAddresses(conn);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var address = new Address()
                    {
                        Id = reader.GetGuid(0),
                        Zip = reader.GetString(1),
                        City = reader.GetString(2),
                        Street = reader.GetString(3),
                        Number = reader.GetString(4),
                        CountryCode = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6)
                    };
                    addresses.Add(address);
                }
            }
            
            reader.Close();
            
            conn.Close();
            
            return addresses.ToArray();
        }
        
        private SqlDataReader SelectAddresses(SqlConnection conn)
        {
            try
            {
                string query =
                    @"SELECT Id, Zip, City, Street, Number, CountryCode, CreatedAt  FROM Addresses";

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