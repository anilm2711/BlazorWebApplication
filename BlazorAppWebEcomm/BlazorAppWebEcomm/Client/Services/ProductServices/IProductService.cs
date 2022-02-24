namespace BlazorAppWebEcomm.Client.Services.ProductServices
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        Task GetProducts();
        Task<ServiceResponse<Product>> GetProduct(int productId);

        Task GetProductByCategory(string categoryUrl);
    }
}
