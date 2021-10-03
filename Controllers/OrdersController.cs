using InmetaTest.Dtos;
using InmetaTest.Helpers;
using InmetaTest.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using InmetaTest.Entities;

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

        //GET /orders/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<OrderDto> GetOrder(Guid id)
        {
            var order = repository.GetOrder(id);
            if (order is null)
            {
                return new NotFoundResult();
            }
            return order.AsDto();
        }

        //POST /orders
        [HttpPost]
        public ActionResult<OrderDto> CreateOrder(OrderDto orderDto)
        {
            Order order = new Order()
            {
                Id = Guid.NewGuid(),
                
            };
            repository.CreateOrder(orderDto);
            return orderDto;
        }
        
        //PUT /orders/{id}
        [HttpPut("{id:guid}")]
        public ActionResult<OrderDto> UpdateOrder(Guid id,OrderDto orderDto)
        {
            var existingOrder = repository.GetOrder(id);
            if (existingOrder is null)
            {
                return new NotFoundResult();
            }
            repository.UpdateOrder(orderDto);
            return orderDto;
        }
        
        //DELETE /orders/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult DeleteOrder(Guid id)
        {
            var existingOrder = repository.GetOrder(id);
            if (existingOrder is null)
            {
                return new NotFoundResult();
            }
            repository.DeleteOrder(id);
            return new NoContentResult();
        }
    }
}