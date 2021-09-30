using InmetaTest.Dtos;
using InmetaTest.Helpers;
using InmetaTest.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmetaTest.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControlerBase
    {
        private readonly IOrdersRepository repository;

        public OrdersController(IOrdersRepository repository)
        {
            this.repository = repository;
        }

        //GET /orders
        [HttpGet]
        public IEnumerable<OrderDto> GetOrders()
        {
            var orders = repository.GetOrders().Select(order => order.AsDto());
            return orders;
        }

        //GET /orders
        [HttpGet("{id}")]
        public ActionResult<OrderDto> GetOrder(Guid id)
        {
            var order = repository.GetOrder(id);
            //if (order is null)
            //{
            //    return NotFound();
            //}
            return order.AsDto();
        }

    }
}
