
namespace BlazorAppWebEcomm.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Models.Category>>> GetCategoriesAsync();
        Task<ServiceResponse<Models.Category>> GetCategoryAsync(int productId);
    }
}
