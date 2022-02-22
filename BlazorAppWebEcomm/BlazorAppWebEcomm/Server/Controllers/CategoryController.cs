using BlazorAppWebEcomm.Server.Services.CategoryServices;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private  ICategoryService _categoryService { get; set; }

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Models.Category>>>> GetCategories()
        {
            ServiceResponse<List<Models.Category>>? categories =await  _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<ActionResult<ServiceResponse<Models.Category>>> GetCategory(int categoryId)
        {
            ServiceResponse<Models.Category>? category = await _categoryService.GetCategoryAsync(categoryId);
                return Ok(category);
        }
    }
}
