using BlazorAppWebEcomm.Server.Models;

namespace BlazorAppWebEcomm.Server.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Models.Product>>> GetProductsAsync();
        Task<ServiceResponse<Models.Product>> GetProductAsync(int productId);
        Task<ServiceResponse<List<Models.Product>>> GetProductByCategoryAsync(string categoryUrl);
        Task<ServiceResponse<ProductSearchResult<Models.Product>>> SearchProducts(string searchText,int page);

        Task<ServiceResponse<List<string>>> GetProductSearchSuggestion(string searchText);
        Task<ServiceResponse<List<Models.Product>>> GetFeaturedProductsAsync();
    }
}
