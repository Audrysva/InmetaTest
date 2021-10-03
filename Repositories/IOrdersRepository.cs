using InmetaTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InmetaTest.Controllers;
using InmetaTest.Dtos;

namespace InmetaTest.Repositories
{
    public interface IOrdersRepository
    {
        Order GetOrder(Guid id);
        IEnumerable<Order> GetOrders();
        void CreateOrder(OrderDto order);
        void UpdateOrder(OrderDto order);
        void DeleteOrder(Guid id);

    }
}
