using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmetaTest.Dtos;

namespace InmetaTest.Repositories
{
    public interface IOrdersRepository
    {
        Order GetOrder(Guid id);
        IEnumerable<Order> GetOrders();

        void CreateOrder(Order order);

    }
}
