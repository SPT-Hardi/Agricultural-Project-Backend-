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

        [HttpGet("ViewAllArea")]
        public async Task<IActionResult> ViewAllAreaAsync()
        {
            var result = _areaRepository.ViewAllArea();
            return Ok(result);
        }

        [HttpPost("addMainArea")]
        public async Task<IActionResult> AddMainArea(AreaModel mainAreaModel)
        {
            var result = _areaRepository.AddMainAreaAsync(mainAreaModel);
            return Ok(result);
        }
        [HttpGet("MacAddress")]
        public async Task<IActionResult> ActionResultAsync()
        {
            var result = _areaRepository.GetMacAddress();
            return Ok(result);
        }

        [HttpPut("EditArea/{mid}/{sid}")]
        public async Task<IActionResult> EditAreaAsync(UpdateAreaModel value, int mid, int sid)
        {
            var result = _areaRepository.EditArea(value, mid,sid);
            return Ok(result);
        }
    }
}
