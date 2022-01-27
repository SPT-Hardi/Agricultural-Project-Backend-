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
        private readonly IAreaRepository _mainAreaRepository;

        public AreaController(IAreaRepository mainAreaRepository)
        {
            _mainAreaRepository = mainAreaRepository;
        }

        [HttpPost("addMainArea")]
        public async Task<IActionResult> AddMainArea(MainAreaModel mainAreaModel)
        {
            var result = _mainAreaRepository.AddMainAreaAsync(mainAreaModel);
            return Ok(result);
        }

       
    }
}
