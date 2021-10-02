using InmetaTest.Entities;
using System.Collections.Generic;

namespace InmetaTest.Repositories
{
    public interface ICustomersRepository
    {
        IEnumerable<Customer> GetCustomers();
    }
}
