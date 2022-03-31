
namespace BlazorAppWebEcomm.Server.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse<List<Models.Category>>> GetCategoriesAsync();
        Task<ServiceResponse<Models.Category>> GetCategoryAsync(int productId);
        Task<ServiceResponse<List<Models.Category>>> GetAdminCategories();
        Task<ServiceResponse<List<Models.Category>>> AddCategory(Models.Category category);
        Task<ServiceResponse<List<Models.Category>>> UpdateCategory(Models.Category category);
        Task<ServiceResponse<List<Models.Category>>> DeleteCategory(int Id);


    }
}
