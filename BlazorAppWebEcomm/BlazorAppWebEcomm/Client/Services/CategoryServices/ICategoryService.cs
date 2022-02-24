namespace BlazorAppWebEcomm.Client.Services.CategoryServices
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetCategories();
        Task<ServiceResponse<Category>> GetProduct(int categoryId);
    }
}
