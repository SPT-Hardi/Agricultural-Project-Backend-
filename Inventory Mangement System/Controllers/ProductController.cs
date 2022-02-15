using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        //View All Product
        [HttpGet("ViewAllProduct")]
        public async Task<IActionResult> ViewAllProductAsync()
        {
            var result =  _productRepository.ViewAllProduct();
            return Ok(result);
        }

        //Add Product
        [HttpPost ("addproduct")]
       // [Authorize(Roles = "Super Admin,Admin")]
        public async Task<IActionResult> ProductAdded(ProductModel productModel)
        {
            var result = _productRepository.AddProduct(productModel);
            return Ok(result);
           
        }

        //View Product By Id
        [HttpGet("ViewProductById/{productID}")]
        public async Task<IActionResult> ViewProductByIDAsync([FromRoute] int productID)
        {
            var result = _productRepository.ViewProductById(productID);
            return Ok(result);
        }

        //Edit Product Using Put Method
        [HttpPut("EditProduct/{productID}")]
        public async Task<IActionResult> EditProductAsync([FromBody] ProductDetail productDetail, [FromRoute] int productID)
        {
            var result = _productRepository.EditProduct(productDetail, productID);
            return Ok(result);
        }
      
        //Get Unit
        [HttpGet("getunit")]
        public async Task<IActionResult> ProductGet()
        {
            var result = await _productRepository.GetUnit();
            return Ok(result);
        }

        [HttpPatch("UpdateProduct/{productID}")]
        public async Task<IActionResult> Update([FromBody] JsonPatchDocument productModel, [FromRoute] int productID)
        {
            var result = _productRepository.UpdateProduct(productModel, productID);
            return Ok(result);
        }
    }
}
