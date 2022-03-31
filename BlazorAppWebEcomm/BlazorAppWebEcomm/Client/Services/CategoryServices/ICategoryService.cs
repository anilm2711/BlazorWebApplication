namespace BlazorAppWebEcomm.Client.Services.CategoryServices
{
    public interface ICategoryService
    {
        event Action OnChange;
        List<Category> Categories { get; set; }
        List<Category> AdminCategories { get; set; }
        Task<List<Category>> GetAdminCategories();
        Task GetCategories();
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int Id);
        Task<ServiceResponse<Category>> GetProduct(int categoryId);
        Category CreateNewCategory();

    }
}
