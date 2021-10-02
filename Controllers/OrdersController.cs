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
        
        //POST /orders
        [HttpPost]
        public ActionResult<OrderDto> CreateOrder(CreateOrderDto orderDto)
        {
            List<Service> products = new();
            if (orderDto.Products.Count == 0)
            {
                throw new NullReferenceException();
            }

            Order order = new Order()
            {
                Id = new Guid(),
                Services = orderDto.Products
            };
            
            repository.CreateOrder(order);
            return order.AsDto();
        }

    }

    public class CreateOrderDto
    {
        public List<Service> Products { get; set; }
    }
}
