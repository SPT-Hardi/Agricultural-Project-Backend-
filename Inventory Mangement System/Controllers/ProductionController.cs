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
    public class ProductionController : ControllerBase
    {
        private readonly IProductionRepository _productionRepository;

        public ProductionController(IProductionRepository productionRepository)
        {
            _productionRepository = productionRepository;
        }

        //View All Production Details 
        [HttpGet("viewproductiondetail")]
        public async Task<IActionResult> ViewProductionDetail()
        {
            var result = _productionRepository.ViewAllProductionDetails();
            return Ok(result);
        }

        //Add Production Details
        [HttpPost("addproduction")]
        public async Task<IActionResult> ProductionDetailAdded([FromBody] ProductionModel productionModel)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _productionRepository.AddProductionDetails(productionModel,LoginId);
            return Ok(result);

        }

        //Edit Production
        [HttpPut("Editproduction/{id}")]
        public async Task<IActionResult> EditProduction(ProductionModel productionModel, int id)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _productionRepository.Editproduction(productionModel, id,LoginId);
            return Ok(result);
        }
    }
}
