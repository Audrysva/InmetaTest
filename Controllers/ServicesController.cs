using System;
using System.Collections.Generic;
using System.Linq;
using InmetaTest.Entities;
using Microsoft.AspNetCore.Mvc;

namespace InmetaTest.Controllers
{
    [ApiController]
    [Route("services")]
    
    public class ServicesController: ControllerBase
    {
        //GET /services/types
        [HttpGet("types")]
        public Dictionary<int,string> GetAddresses()
        {
            var dict = new Dictionary<int, string>();
            foreach (var name in Enum.GetNames(typeof(EServiceTypes)))
            {
                dict.Add((int)Enum.Parse(typeof(EServiceTypes), name), name);
            }
            return dict;
        }
        
    }
}