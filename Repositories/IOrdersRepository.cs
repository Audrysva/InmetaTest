using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmetaTest.Repositories
{
    public interface IOrdersRepository
    {
        Order GetOrder(Guid id);
        IEnumerable<Order> GetOrders();
        
    }
}
