using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Model.Common;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Inventory_Mangement_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository )
        {
            _categoryRepository = categoryRepository;
        }

        //View Category
        [HttpGet("viewCategory")]
        public async Task<IActionResult> CategoryView()
        {
            var result = _categoryRepository.ViewCategory();
            return Ok(result);
        }

        //Add New Category
        [HttpPost("addCategory")]
        public async Task<IActionResult> CategoryAdded(CategoryModel categoryModel)
        {
            if((int)HttpContext.Items["LoginId"] ==0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _categoryRepository.AddCategory(categoryModel,LoginId);
            return Ok(result);
        }

        //Edit Category     
        [HttpPut("EditCategory/{id}")]
        public async Task<IActionResult> CategoryEdit(CategoryModel categoryModel, int id)
        {
            if ((int)HttpContext.Items["LoginId"] == 0)
            {
                throw new ArgumentException("JWT Token Not Found.");
            }
            int LoginId = (int)HttpContext.Items["LoginId"];
            var result = _categoryRepository.EditCategory(categoryModel, id,LoginId);
            return Ok(result);
        }

        //DropDown For Category
        [HttpGet("getCategory")]
        public async Task<IActionResult> CategoryGet()
        {
            var result = await _categoryRepository.GetCategory();
            return Ok(result);
        }

        //View Category With Paging
        [HttpGet("viewCategorys")]
        public async Task<IActionResult> CategoryViews([FromQuery] Paging value)
        {
            var result = _categoryRepository.ViewCategorys(value);
            return Ok(result);
        }

        
    }
}
