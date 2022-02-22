using BlazorAppWebEcomm.Server.Models;

namespace BlazorAppWebEcomm.Server.Services.ProductServices
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Models.Product>>> GetProductsAsync();
        Task<ServiceResponse<Models.Product>> GetProductAsync(int productId);    
    }
}
