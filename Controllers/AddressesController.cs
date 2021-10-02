using System.Collections.Generic;
using InmetaTest.Entities;
using InmetaTest.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InmetaTest.Controllers
{
    [ApiController]
    [Route("addresses")]
    
    public class AddressesController: ControllerBase
    {
        private readonly IAddressesRepository repository;
        
        public AddressesController(IAddressesRepository repository)
        {
            this.repository = repository;
        }
        
        //GET /addresses
        [HttpGet]
        public IEnumerable<Address> GetAddresses()
        {
            var addresses = repository.GetAddresses();
            return addresses;
        }
        
    }
}