using System.Collections.Generic;
using InmetaTest.Dtos;
using InmetaTest.Entities;
using InmetaTest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InmetaTest.Controllers
{
    [ApiController]
    [Route("customers")]
    
    public class CustomersController: ControllerBase
    {
        private readonly ICustomersRepository repository;
        
        public CustomersController(ICustomersRepository repository)
        {
            this.repository = repository;
        }
        
        //GET /customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            var customers = repository.GetCustomers();
            return customers;
        }
        
    }
}