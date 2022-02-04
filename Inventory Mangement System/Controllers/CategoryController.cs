using Inventory_Mangement_System.Model;
using Inventory_Mangement_System.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("addCategory")]
        public async Task<IActionResult> CategoryAdded(CategoryModel categoryModel)
        {
             var result = _categoryRepository.AddCategory(categoryModel);
             return Ok(result);
        }

        [HttpGet("getCategory")]
        public async Task<IActionResult> CategoryGet()
        {
            var result = await _categoryRepository.GetCategory();
            return Ok(result);
        }

        [HttpGet("viewCategory")]
        public async Task<IActionResult> CategoryView()
        {
            var result = await _categoryRepository.ViewCategory();
            return Ok(result);
        }

        [HttpGet("viewCategoryById/{id}")]
        public async Task<IActionResult> CategoryViewById(int id)
        {
            var result = await _categoryRepository.ViewCategoryById(id);
            return Ok(result);
        }

        [HttpPut("editCategory/{id}")]
        public async Task<IActionResult> CategoryEdit(CategoryModel categoryModel,int id)
        {
            var result = _categoryRepository.EditCategory(categoryModel,id);
            return Ok(result);
        }

    }
}
