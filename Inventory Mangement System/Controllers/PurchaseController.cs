﻿using Inventory_Mangement_System.Model;
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

        //View Purchase Details
        [HttpGet("getpurchaseproduct")]
        public async Task<IActionResult> GetPurchaseDetails()
        {
            var result = _purchaseRepository.GetPurchaseDetails();

            return Ok(result);
        }

        //Add Purchase Details
        [HttpPost("purchaseproduct")]
        public async Task<IActionResult> PurchaseDetailsAdded(PurchaseModel purchaseModel)
        {
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _purchaseRepository.AddPurchaseDetails(purchaseModel,LoginId);

            return Ok(result);
        }

        //Edit Purchase Details
        [HttpPut("EditPurchaseProduct/{ID}")]
        public async Task<IActionResult> EditPurchaseProduct(PurchaseModel purchaseModel,int ID)
        {
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _purchaseRepository.EditPurchaseProduct(purchaseModel,ID,LoginId);

            return Ok(result);
        }
    }
}
