using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;

        public AreaController(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        //View All Main And Sub Area
        [HttpGet("ViewAllArea")]
        public async Task<IActionResult> ViewAllAreaAsync([FromQuery]int? Id)
        {
            var result = _areaRepository.ViewAllArea(Id);
            return Ok(result);
        }

        //Add New Main And Sub Area
        [HttpPost("addMainArea")]
        public async Task<IActionResult> AddMainArea(AreaModel mainAreaModel)
        {
            var LoginId = HttpContext.Items["LoginId"];
            var result = _areaRepository.AddMainAreaAsync(mainAreaModel,LoginId);
            return Ok(result);
        }

        //Edit Main And Sub Area
        [HttpPut("EditArea/{Id}")]
        public async Task<IActionResult> EditAreaAsync(UpdateAreaModel value, int Id)
        {
            var LoginId = HttpContext.Items["LoginId"];
            var result = _areaRepository.EditArea(value, Id,LoginId);
            return Ok(result);
        }
        
    }
}
