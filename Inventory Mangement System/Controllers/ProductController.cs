using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
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

        //Add Product Details
        [HttpPost ("addproduct")]
        public IActionResult ProductAdded(ProductModel productModel)
        {
            var result = _productRepository.AddProduct(productModel);
            return Ok(result);
        }

        //Get Unit
        [HttpGet("getunit")]
        public async Task<IActionResult> ProductGet()
        {
            var result = _productRepository.GetUnit();
            return Ok(result.Result);
        }

        //Get Product Detail
        [HttpGet("ViewAllProduct")]
        public async Task<IActionResult> ViewAllProductAsync()
        {
            var result = await _productRepository.ViewAllProduct();
            return Ok(result);
        }

        //Get Product By ID
        [HttpGet("ViewProductById/{id}")]
        public async Task<IActionResult> ViewProductByIDAsync([FromRoute]int id)
        {
            var result = await _productRepository.ViewProductById(id);
            return Ok(result);
        }

        //Edit Product
        [HttpPut("EditProduct/{id}")]
        public async Task<IActionResult> EditProductAsync([FromBody]ProductDetail productDetail, [FromRoute] int id)
        {
            var result = _productRepository.EditProduct(productDetail,id);
            return Ok(result);
        }
    }
}
