namespace BlazorAppWebEcomm.Client.Services.ProductServices
{
    public interface IProductService
    {
        event Action ProductsChanged;
        List<Product> Products { get; set; }

        string Message { get; set; }
        Task GetProducts();
        Task<ServiceResponse<Product>> GetProduct(int productId);

        Task SearchProducts(string  searchText);

        Task GetProductByCategory(string categoryUrl);

        Task<List<string>> GetProductSearchSuggestion(string searchText);
    }
}
