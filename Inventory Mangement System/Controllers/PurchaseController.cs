using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
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
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository _purchaseRepository;

        public PurchaseController (IPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        [HttpGet("GetunitByid/{id}")]
        public async Task<IActionResult> UnitById(int id)
        {
            var result = await _purchaseRepository.GetunitByid(id);

            return Ok(result);
        }
        [HttpPost("purchaseproduct")]
        public async Task<IActionResult> PurchaseDetailsAdded(PurchaseModel purchaseModel)
        {
            var result = _purchaseRepository.AddPurchaseDetails(purchaseModel);

            return Ok(result);
        }

    }
}
