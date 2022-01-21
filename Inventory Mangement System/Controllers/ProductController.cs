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
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController (IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost ("addproduct")]
        public IActionResult ProductAdded(ProductModel productModel)
        {
           
            var  result =  _productRepository.AddProduct(productModel);
            return Ok(result);
        }

        [HttpGet("getunit")]
        public async Task<IActionResult> ProductGet()
        {
            var result = await _productRepository.GetUnit();
            return Ok(result);
        }
    }
}
