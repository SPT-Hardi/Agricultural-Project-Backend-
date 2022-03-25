﻿using Inventory_Mangement_System.Repository;
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
    public class InventoryViewsController : ControllerBase
    {
        private readonly IInventoryViewRepository _inventoryViewRepository;

        public InventoryViewsController(IInventoryViewRepository inventoryViewRepository)
        {
            _inventoryViewRepository = inventoryViewRepository;
        }

        //View Purchase Details
        [HttpGet("GetInventoryView")]
        public async Task<IActionResult> GetInventoryView()
        {
            var result = _inventoryViewRepository.GetInventoryView();
            return Ok(result);
        }
    }
}
