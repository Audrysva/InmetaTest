using System.Collections.Generic;
using InmetaTest.Entities;

namespace InmetaTest.Repositories
{
    public interface IAddressesRepository
    {

            IEnumerable<Address> GetAddresses();
    }
}