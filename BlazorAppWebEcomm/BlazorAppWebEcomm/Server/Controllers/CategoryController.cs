using BlazorAppWebEcomm.Server.Services.CategoryServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppWebEcomm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryService _categoryService { get; set; }

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Models.Category>>>> GetCategories()
        {
            ServiceResponse<List<Models.Category>>? categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<ActionResult<ServiceResponse<Models.Category>>> GetCategory(int categoryId)
        {
            ServiceResponse<Models.Category>? category = await _categoryService.GetCategoryAsync(categoryId);
            return Ok(category);
        }

        [HttpGet("getadmincategories"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Models.Category>>>> GetAdminCategories()
        {
            var result = await _categoryService.GetAdminCategories();
            return Ok(result);
        }

        [HttpPost("addcategory"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Models.Category>>>> AddCategory(Models.Category category)
        {
            var result = await _categoryService.AddCategory(category);
            return Ok(result);
        }
        [HttpPost("deletecategory/{Id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Models.Category>>>> DeleteCategory(int Id)
        {
            var result = await _categoryService.DeleteCategory(Id);
            return Ok(result);
        }

        [HttpPut("updatecategory"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Models.Category>>>> UpdateCategory(Models.Category category)
        {
            var result = await _categoryService.UpdateCategory(category);
            return Ok(result);

        }
    }
}
