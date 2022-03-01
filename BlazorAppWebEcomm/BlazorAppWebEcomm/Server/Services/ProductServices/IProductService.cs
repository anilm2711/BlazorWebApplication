using BlazorAppWebEcomm.Server.Models;

namespace BlazorAppWebEcomm.Server.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Models.Product>>> GetProductsAsync();
        Task<ServiceResponse<Models.Product>> GetProductAsync(int productId);
        Task<ServiceResponse<List<Models.Product>>> GetProductByCategoryAsync(string categoryUrl);
        Task<ServiceResponse<List<Models.Product>>> SearchProducts(string searchText);

        Task<ServiceResponse<List<string>>> GetProductSearchSuggestion(string searchText);
    }
}
