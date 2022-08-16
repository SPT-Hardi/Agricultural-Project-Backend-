using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{

    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("test")]
        public IActionResult Test()
        {
            return Ok("Working!!");
        }

        [HttpGet]
        [Route("date")]
        public IActionResult DateValue()
        {
            return Ok(new Repository.ISDT().DateValue());
        }

        [HttpPut]
        [Route("put")]
        public IActionResult TestPut() 
        {
            return Ok("Working!!");
        }
    }
}
