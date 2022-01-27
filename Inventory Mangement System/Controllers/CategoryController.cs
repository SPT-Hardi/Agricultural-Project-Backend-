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

        [Authorize(Roles = "1")]
        [HttpPost("addCategory")]
        public async Task<IActionResult> CategoryAdded(CategoryModel categoryModel)
        {
            int uid = (int)HttpContext.Items["UserId"];
            
                var result = _categoryRepository.AddCategory(categoryModel,uid);
                return Ok(result);
        }


        [HttpGet("getCategory")]
        public async Task<IActionResult> CategoryGet()
        {
            var result = await _categoryRepository.GetCategory();
            return Ok(result);
        }

    }
}
