using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/Vegetables")]
    [ApiController]
    public class VegetablesController : ControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public IActionResult Post([FromBody]Model.Vegetable value) 
        {
            var LoginId = HttpContext.Items["LoginId"];
            return Ok(new Vegetables().AddList(value,LoginId));
        }

        [HttpGet]
        [Route("Drop")]
        public IActionResult Drop()
        {
            return Ok(new Vegetables().Drop());
        }

        [HttpGet]
        [Route("View")]
        public IActionResult Get()
        {
            return Ok(new Vegetables().View());
        }

        [HttpPut]
        [Route("Update/{Id}")]
        public IActionResult Patch([FromBody]Model.Vegetable value,[FromRoute]int Id)
        {
            var LoginId = HttpContext.Items["LoginId"];
            return Ok(new Vegetables().Update(Id,value,LoginId));
        }

        [HttpDelete]
        [Route("Delete/{Id}")]
        public IActionResult Delete([FromRoute]int Id)
        {
            var LoginId = HttpContext.Items["LoginId"];
            return Ok(new Vegetables().Delete(Id,LoginId));
        }
    }
}
