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
        public async Task<IActionResult> ViewAllAreaAsync()
        {
            var result = _areaRepository.ViewAllArea();
            return Ok(result);
        }

        //Add New Main And Sub Area
        [HttpPost("addMainArea")]
        public async Task<IActionResult> AddMainArea(AreaModel mainAreaModel)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _areaRepository.AddMainAreaAsync(mainAreaModel,LoginId);
            return Ok(result);
        }

        //Edit Main And Sub Area
        [HttpPut("EditArea/{mid}/{sid}")]
        public async Task<IActionResult> EditAreaAsync(UpdateAreaModel value, int mid, int sid)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _areaRepository.EditArea(value, mid,sid,LoginId);
            return Ok(result);
        }
        
    }
}
